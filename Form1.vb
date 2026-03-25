Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

#Region "Info/Comments"
' Este Programa Fue Escrito Para ser usando como Base del Launcher Original de Axeso5 Para Ingresar Al Operation7
' Esta Recreacion es Basada En la original (Puede Ser Utilizado Como Base Cambiando Unas Cuantas Lineas.

' Discord: "VassaMadness o Vassa Madness#7824" - (Dudas, Sugerencias, Actualizaciones).

' ¿Porque Se Publico?
' R: Para Los Curiosos, La exploracion del funcionamiento De Un Launcher, Para Los Que Quieran Actualizarlo, Para Los Que Busquen Aprender.
' Se Publica Al Aver Concluido El Proyecto zXXXXX, Se Agregaron Mejoras A Este Launcher Que Originalmente Faltaban En El Original De Axeso5 (Guardado Automatico de Cuentas, Ayudantes, )
#End Region

Public Class Form1
    Dim Datos As String = Application.StartupPath & "\settings.cfg" 'Se Declara la funcion Guardado y Ruta. (Originalmente No Existe En Ax5)
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vkey As Integer) As Integer
    Private Function MakeHash(ByVal input As String) As String 'Convertimos Los Datos Al Codigo HASH de la web, Necesario Actualizar Para Funcionar Hoy en dia. (Mismo que CRC)
        Dim a As MD5 = MD5.Create()
        Dim b As Byte() = a.ComputeHash(Encoding.Default.GetBytes(input))
        Dim c As New StringBuilder()
        Dim i As Integer

        For i = 0 To b.Length - 1
            c.Append(b(i).ToString("x2"))
        Next i
        Return c.ToString()
    End Function

    <Runtime.InteropServices.DllImport("wininet.dll", SetLastError:=True)> _
    Private Shared Function InternetSetOption(ByVal hInternet As IntPtr, ByVal dwOption As Integer, ByVal lpBuffer As IntPtr, ByVal lpdwBufferLength As Integer) As Boolean
    End Function
    Private Sub LabelMover_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelMover.Click
        Process.Start("http://www.axeso5.com/registro")
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        Me.Close()
        'Form4.Close() 'Formulario Principal
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'Funcion Escritura Tipo Texto De La Informacion Perteneciente A Usuario & Contraseña
        My.Computer.FileSystem.WriteAllText(Datos, "[LOGIN]" & vbCrLf & TextBox1.Text & vbCrLf & TextBox2.Text, False)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try 'Intentamos la lectura de los archivos escritos, Usuario & Contraseña
            TextBox1.Text = File.ReadLines(Datos)(1)
            TextBox2.Text = File.ReadLines(Datos)(2)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ' Timer Main, Registra Lo Realizado En Tiempo Real Del Programa

        Me.Text = "zClauncher - " + TimeOfDay
        TextBox3.Text = "&password=" & MakeHash(TextBox2.Text)

        Dim HK1 As Boolean
        HK1 = GetAsyncKeyState(Keys.Enter)
        If HK1 = True Then
            Button1.Show()
            Button1.Select()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        'Usuario'
        If TextBox1.Text = "" Then
            Fail.Show()
        Else
            CheckBox1.Checked = True
        End If

        'Contraseña'
        If TextBox2.Text = "" Then
            Fail.Show()
        Else
            CheckBox1.Checked = True
        End If

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.CheckState = CheckState.Checked Then
            wb.Navigate("http://209.251.184.23/services/op7auth.ashx?loginid=" & TextBox1.Text + TextBox3.Text) 'Antigua Auth En Axeso5, Es Necesario Actualizar.
            Timer2.Enabled = True
            CheckBox1.Checked = False
        Else
            CheckBox1.Checked = False
        End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        'Filtrado HTML
        Try
            Dim MIIP As String = wb.DocumentText
            MIIP = MIIP.Remove(0, MIIP.IndexOf("otp=" & "") + 4)
            TextBox4.Text = MIIP.Substring(0, MIIP.IndexOf("<"))
        Catch ex As Exception
        End Try
        CheckBox2.Checked = True
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        Dim A As String
        A = Shell(TextBox5.Text + TextBox1.Text + " " + TextBox4.Text, AppWinStyle.NormalFocus) 'Llamada Al Juego Con Verificacion Web
        Me.Close()
        'Form3.Show() 'Funcion Extra Buscar (Obsoleta y eliminada) - Buscador De IDs
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        MsgBox("Ups!! Parece Que El Teclado No Esta Disponible.", MsgBoxStyle.AbortRetryIgnore, "Error En El Teclado")
    End Sub

    Private Sub AbrirToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AbrirToolStripMenuItem.Click
        Me.Show()
    End Sub

    Private Sub SalirToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalirToolStripMenuItem.Click
        Me.Close()
    End Sub

    'Funcion Extra Buscar (Obsoleta y eliminada) - Buscador De IDs
    Private Sub BuscadorDeIDsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuscadorDeIDsToolStripMenuItem.Click
        'Form3.Show()
    End Sub

    'Funcion extra minimizar el archivo
    Private Sub CheckBox2_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox2.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Hide()
            NotifyIcon1.BalloonTipText = "El Programa Se Ah Minimizado"
            NotifyIcon1.BalloonTipTitle = "Launcher v1.0"
            NotifyIcon1.Visible = True
            NotifyIcon1.ShowBalloonTip(0)
        End If
    End Sub
End Class

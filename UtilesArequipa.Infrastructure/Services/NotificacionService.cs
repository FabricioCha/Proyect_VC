namespace UtilesArequipa.Infrastructure.Services;

public class NotificacionService
{
    public void EnviarNotificacionNuevoKit(string nombreKit)
    {
        Console.WriteLine($"[Notificación] Nuevo Kit Creado: {nombreKit} - Enviando correo a administradores...");
        // Simulación de tarea pesada
        Thread.Sleep(2000); 
        Console.WriteLine($"[Notificación] Correo enviado exitosamente para el kit: {nombreKit}");
    }
}

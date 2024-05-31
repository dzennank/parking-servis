using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ParkingServis.Client
{
    public class MessageNotification
    {
        public void HandleMessagesNotification(Snackbar clientSB, string message, int duration, SolidColorBrush color)
        {
            clientSB.Background = color;
            clientSB.MessageQueue?.Enqueue(
                message,
                null,
                null,
                null,
                false,
                true,
                TimeSpan.FromSeconds(duration)
            );
        }
    }
}

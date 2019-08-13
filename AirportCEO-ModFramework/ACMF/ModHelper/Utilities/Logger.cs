using System;
using System.Collections.Generic;

namespace ACMF.ModHelper.Utilities
{
    public class Logger
    {
        public static void Error(string message, bool alertInGame = false) => Print($"[ERROR] {message}", alertInGame);
        public static void Print(string message, bool alertInGame = false)
        {
            Console.WriteLine($"[AirportCEOModFramework] {message}");

            if (alertInGame)
                ShowNotification(message);
        }

        public static void Print<T>(ICollection<T> collection)
        {
            Print($"Printing {collection} || Size {collection.Count}");
            int no = 1;
            foreach (T t in collection)
            {
                Print($"    {no}: {t}");
                no++;
            }
        }

        public static void Print<T>(IEnumerable<T> enumerable)
        {
            Print($"Printing {enumerable}");
            int no = 1;
            foreach(T t in enumerable)
            {
                Print($"    {no}: {t}");
                no++;
            }
        }

        public static void Print<T>(T[] array)
        {
            Print($"Printing {array}");
            int no = 1;
            foreach (T t in array)
            {
                Print($"    {no}: {t}");
                no++;
            }
        }

        public static void Print(Array array)
        {
            Print($"Printing {array}");
            int no = 1;
            foreach (object t in array)
            {
                Print($"    {no}: {t}");
                no++;
            }
        }

        public static void ShowNotification(string message)
        {
            if (NotificationController.Instance != null)
                NotificationController.Instance.SendUINotification(message, Enums.NotificationType.Other, Enums.MessageSeverity.Unspecified, "", true, true);

            Console.WriteLine($"[Logger::ShowNotification] {message}");
        }
    }
}

using System;
using System.Diagnostics;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace ConsoleApp1
{
    class Program
    {
        private static ChatId chatId;
        private static CancellationToken cancellationToken;

        static void Main(string[] args)
        {
            var client = new TelegramBotClient("6221793637:AAHhw1HAgy3ohj2-6Nb-BCj2_shOFCniNbQ");
            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;

            if (message.Text != null)
            {
                Console.WriteLine($"{message.Chat.FirstName}    |   {message.Text}");
                if (message.Text.Contains("hello") || message.Text.Contains("Hello") || message.Text.Contains("hi") || message.Text.Contains("Hi"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Hello my dear friend, I can make you photo better:) Can you send the photo for me?");
                    return;
                }

                if (message.Text.Contains("how are you?"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "I'm cool. Are you?");
                    return;
                }
            }

            if (message.Photo != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Сan you send the photo as a file?");
                return;
            }

            if (message.Document != null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Wait please, i'm working...");

                var fileId = update.Message.Document.FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;

                string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{message.Document.FileName}";

                await using Stream fileStream = System.IO.File.OpenWrite(destinationFilePath);
                await botClient.DownloadFileAsync(filePath, fileStream);
                fileStream.Close();

                Process.Start(@"C:\Users\Admin\Deskop\MovieStar.exe", $@"""{destinationFilePath}""");

                await using Stream stream = System.IO.File.OpenRead(destinationFilePath);
                await botClient.SendDocumentAsync(message.Chat.Id, new InputOnlineFile(stream, message.Document.FileName.Replace(".jpg", "edited.jpg")));


                return;
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
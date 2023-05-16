using System.Media;

class Program
{
    static void Main()
    {
        int secondsDelay = 64;

        DateTime timeToPlayAlarm = DateTime.Now;    //заносим текущее время
        timeToPlayAlarm = timeToPlayAlarm.AddSeconds(secondsDelay);             //просто для примера добавляем 10 секунд - то есть будильник зазвонит через 10 секунд после запуска программы

        /*//устанавливаем сегодняшний день и заданное этими двумя переменными время
        int hour = 18;
        int minute = 59;

        DateTime now = DateTime.Now;
        DateTime timeToPlayAlarm = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);*/

        //пока не выполнится условие - просто ждем и каждую секунду пишем в консоль сколько времени осталось до будильника
        while (!shouldPlayAlarm(timeToPlayAlarm))
        {
            logAlarm(timeToPlayAlarm);
            Thread.Sleep(1000);
            continue;
        }
        //теперь включаем будильник
        playAlarmSound();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    private static void logAlarm(DateTime timeToPlayAlarm)
    {
        TimeSpan difference = timeToPlayAlarm - DateTime.Now;

        double seconds = difference.TotalSeconds;
        seconds = Math.Round(seconds, 1);

        //из seconds сюда сформировать красивое представление времени в формате:
        // 3661с -> 1ч 1м 1с
        string timeLeft = secondsToPrettyTime(seconds);

        Console.WriteLine("Осталось " + timeLeft + "с до будильника.");
    }

    // 3661 -> "1ч 1м 1с"
    private static string secondsToPrettyTime(double totalSeconds)
    {
        int totalMinutes = ((int)totalSeconds / 60);
        int minute = totalMinutes % 60;
        int hour = totalMinutes / 60;
        int second = (int)totalSeconds % 60;

        return $"{hour}ч. {minute}м. {second}";
    }

    //метод который возвращает true, если текущее время больше или равно времени когда надо вкл будильник
    private static bool shouldPlayAlarm(DateTime timeToPlayAlarm)
    {
        return DateTime.Now >= timeToPlayAlarm;
    }

    //метод просто проигрывает звук wav
    private static void playAlarmSound()
    {
        //путь к нему
        string soundFilePath = "C:\\alarm.wav";
        //создаем объект класса SoundPlayer, который будет проигрывать звук. при создании, передаем путь к wav файлу будильника.
        //используем блок using Для того чтобы после проигрывания файла ресурсы освободились из оперативной памяти
        using (SoundPlayer player = new SoundPlayer(soundFilePath))
        {
            //используем конструкцию try-catch чтобы при возникновении ошибки (исключительной ситуации - Exception) в программе
            // программа сразу не ломалась, а мы как то обработали эту ситуацию
            try
            {
                //потенциально ошибко-опасный код, который может бросить исключение (exception (исключительная ситуация (ошибка)))
                player.Play();
            }
            catch (Exception ex)
            {
                //если была выброшена ошибка (Exception) - она будет обработана
                Console.WriteLine("Error playing sound: " + ex.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab12
{

    //--------------------------------------1------------------------------------
    public class SSSlog
    {
        private FileInfo fileInfo;
        public SSSlog(string filePath)
        {
            fileInfo = new FileInfo(filePath);
        }

        //для записи в log.txt
        public void WriteLog(string action, string details)
        {
            string logEntry = $"{DateTime.Now:G} | Action: {action} | Details: {details}";

            try
            {
                using (StreamWriter writer = fileInfo.AppendText())
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  
            }

        }

        //для чтения
        public string ReadLog()
        {
            using (StreamReader writer = fileInfo.OpenText())
            {
                return writer.ReadToEnd();  
            }
        }

        //поиск ключевого слова
        public string SearchInLog(string keywords)
        {
            using (StreamReader reader = fileInfo.OpenText())
            {
                List<string> list = new List<string>(); 
                string line;

                while((line = reader.ReadLine()) != null)
                {
                    if(line.Contains(keywords))
                    {
                        list.Add(line);
                    }
                }

                return list.Count > 0 ? string.Join("\n", list) : "нету нужной информации";
            }
        }

        //---------------------6)--------------------
        public List<string> FilterLog(DateTime? startDate = null, DateTime? endDate = null, string keyword = null)
        {
            List<string> filteredEntries = new List<string>();
            string resultFolderPath = Path.Combine(fileInfo.DirectoryName, "FilteredLogs");

            // Создание папки для хранения результатов, если она еще не существует
            if (!Directory.Exists(resultFolderPath))
            {
                Directory.CreateDirectory(resultFolderPath);
            }

            // Формирование имени файла результата
            string resultFileName = $"FilteredLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string resultFilePath = Path.Combine(resultFolderPath, resultFileName);

            using (StreamReader reader = fileInfo.OpenText())
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Попытка извлечь дату из строки
                    DateTime logDate;
                    if (DateTime.TryParse(line.Split('|')[0].Trim(), out logDate))
                    {
                        bool isInDateRange = (!startDate.HasValue || logDate >= startDate) &&
                                             (!endDate.HasValue || logDate <= endDate);
                        bool containsKeyword = string.IsNullOrEmpty(keyword) || line.Contains(keyword);

                        if (isInDateRange && containsKeyword)
                        {
                            filteredEntries.Add(line);
                        }
                    }
                }
            }

            // Запись результатов в файл
            File.WriteAllLines(resultFilePath, filteredEntries);

            return filteredEntries;
        }
        //---------------------------------------------------
    }
    //---------------------------------------------------------------------------

    //--------------------------------------2------------------------------------
    public class SSSDiskInfo
    {
        //для получения свободного места
        public void GetFreeeSpace(string driveName)
        {
            DriveInfo drive = new DriveInfo(driveName);
            if (drive.IsReady)
            {
                Console.WriteLine($"Диск {drive.Name}: Свободное место: {drive.AvailableFreeSpace / (1024 * 1024 * 1024)} ГБ");
            }
            else
            {
                Console.WriteLine($"Диск {driveName} недоступен.");
            }
        }

        //метод для получения файловой системы
        public void GetFileSystemInfo(string driveName)
        {
            DriveInfo drive = new DriveInfo(driveName);
            if (drive.IsReady)
            {
                Console.WriteLine($"Диск {drive.Name} Файловая система: {drive.DriveFormat}");
            }
            else
            {
                Console.WriteLine($"Диск {driveName} недоступен.");
            }
        }

        //метод для получения информации обо всех дисках
        public void GetAllDrivesInfo()
        {
            DriveInfo[] drives = DriveInfo.GetDrives(); //возвращает имена всех логических дисков компьютера

            foreach (var drive in drives)
            {
                if (drive.IsReady)
                {
                    Console.WriteLine($"Диск: {drive.Name}");
                    Console.WriteLine($"  Общий объем: {drive.TotalSize / Math.Pow(1024, 3)} ГБ");
                    Console.WriteLine($"  Доступный объем: {drive.AvailableFreeSpace / Math.Pow(1024, 3)} ГБ");
                    Console.WriteLine($"  Метка тома: {drive.VolumeLabel}");
                    Console.WriteLine($"  Файловая система: {drive.DriveFormat}");
                }
                else
                {
                    Console.WriteLine($"Диск {drive.Name} недоступен.");
                }
            }
        }
    }

    //---------------------------------------------------------------------------

    //--------------------------------------3------------------------------------
    public class SSSFileInfo
    {
        private FileInfo fileInfo;
        public SSSFileInfo(string filePath)
        {
            fileInfo = new FileInfo(filePath);
        }

        //полный путь файла
        public void GetFullPath()
        {
            Console.WriteLine($"Полный путь файла: {fileInfo.FullName}");
        }

        //размер, расширение и имя
        public void GetFileDetails()
        {
            Console.WriteLine($"Имя файла: {fileInfo.Name}");
            Console.WriteLine($"Размер файла: {fileInfo.Length / 1024.0} КБ");
            Console.WriteLine($"Расширение файла: {fileInfo.Extension}");
        }

        //дата создания и изменения
        public void GetFileDates()
        {
            Console.WriteLine($"Дата создания: {fileInfo.CreationTime}");
            Console.WriteLine($"Дата последнего изменения: {fileInfo.LastWriteTime}");
        }

    }

    //---------------------------------------------------------------------------

    //--------------------------------------4------------------------------------

    public class SSSDirInfo
    {
        private DirectoryInfo dirInfo;
        public SSSDirInfo(string dirPath)
        {
            dirInfo = new DirectoryInfo(dirPath);   
        }

        //метод для получения количества файлов
        public void GetFileCount()
        {
            int fileCount = dirInfo.GetFiles().Length;
            Console.WriteLine($"\nКоличество файлов в директории: {fileCount}");
        }

        //метод для получения времени создания директория
        public void GetCreationTime()
        {
            Console.WriteLine($"Время создания директория: {dirInfo.CreationTime}");
        }

        //метод для получения количества поддиректориев
        public void GetSubdirectoryCount()
        {
            int subdirectoryCount = dirInfo.GetDirectories().Length;
            Console.WriteLine($"Количество поддиректориев: {subdirectoryCount}");
        }

        //метод для получения списка родительских директорий
        public void GetParentDirectories()
        {
            DirectoryInfo parent = dirInfo.Parent;
            Console.WriteLine("Список родительских директорий:");

            if (parent == null)
            {
                Console.WriteLine("Нет родительских директорий.");
            }

            while (parent != null)
            {
                Console.WriteLine(parent.FullName);
                parent = parent.Parent;
            }
        }
    }

    //---------------------------------------------------------------------------

    //--------------------------------------5------------------------------------

    public class SSSFileManager
    {
        //a. 
        public void InspectDisk(string driveName)
        {
            string inspectDir = "SSSInspect";
            string fileName = "sssdirinfo.txt";

            if (Directory.Exists(inspectDir))
            {
                Console.WriteLine("Данная папка уже существует!");
                return;
            }

            //создать директроий 
            Directory.CreateDirectory(inspectDir);

            string filePath = Path.Combine(inspectDir, fileName);

            //получить список файлов и папок и записать в файл
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Список файлов и папок:");
                foreach (var dir in Directory.GetDirectories(driveName))
                {
                    writer.WriteLine("Директория: " + dir);
                }

                foreach (var file in Directory.GetFiles(driveName))
                {
                    writer.WriteLine("Файл: " + file);
                }
            }

            string copyFileName = "sssdirinfo_copy.txt";
            string copyFilePath = Path.Combine(inspectDir, copyFileName);

            File.Copy(filePath, copyFilePath);

            File.Delete(filePath);
        }

        //b
        public void ManageFiles(string sourceDir, string fileExtension)
        {
            if (Directory.Exists("SSSInspect\\SSSFiles"))
            {
                Console.WriteLine($"Файлы уже перемещены в SSSInspect\\SSSFiles. Пропускаем выполнение метода.");
                return; // Завершаем выполнение метода, если файлы уже перемещены
            }

            string filesDir = "SSSFiles";
            Directory.CreateDirectory(filesDir);

            //копировать файлы с заданным расширением
            foreach (var file in Directory.GetFiles(sourceDir, "*" + fileExtension))
            {
                string fileName = Path.GetFileName(file); //получаем только имя файла
                string destPath = Path.Combine(filesDir, fileName);
                File.Copy(file, destPath);
            }

            //переместить SSSFiles в SSSInspect
            string inspectDir = "SSSInspect";
            string targetDir = Path.Combine(inspectDir, filesDir);

            Directory.Move(filesDir, targetDir);
        }

        //c
        public void ArchiveFiles(string sourceDir, string archiveName)
        {
            if (Directory.Exists("Extracted"))
            {
                Console.WriteLine("Архивация и разархивация прошла уже успешно.");
                return;
            }

            string archivePath = archiveName + ".zip";

            // Создать архив
            ZipFile.CreateFromDirectory(sourceDir, archivePath);

            // Разархивировать в другую директорию
            string extractDir = "Extracted";
            Directory.CreateDirectory(extractDir);
            ZipFile.ExtractToDirectory(archivePath, extractDir);
        }
    }

    //---------------------------------------------------------------------------

    //--------------------------------------6------------------------------------







    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n---------------------1)-----------------------");

            SSSlog log = new SSSlog("ssslogfile.txt");

            //фильтрация по дате
            DateTime startDate = new DateTime(2024, 12, 22, 0, 0, 0);
            DateTime endDate = new DateTime(2024, 12, 22, 23, 59, 59);
            List<string> filteredEntries = log.FilterLog(startDate, endDate, "File Created");

            log.WriteLog("File Created", "D:\\My_page\\УНИК\\2 курс\\1 сем\\OOP C#\\Lab12\\Lab12\\Lab12\\bin\\Debug\\ssslogfile.txt");

            Console.WriteLine("Содержимое Log: \n");
            Console.WriteLine(log.ReadLog());

            Console.WriteLine("Поиск нужной информации: \n");
            Console.WriteLine(log.SearchInLog("File Created"));

            Console.WriteLine("\n---------------------2)-----------------------");

            SSSDiskInfo diskInfo = new SSSDiskInfo();

            Console.WriteLine("\nИнформация обо всех доступных дисках:");
            diskInfo.GetAllDrivesInfo();

            Console.WriteLine("\nСвободное место на диске C:");
            diskInfo.GetFreeeSpace("C:");

            Console.WriteLine("\nФайловая система на диске C:");
            diskInfo.GetFileSystemInfo("C:");

            Console.WriteLine("\n---------------------3)-----------------------");

            SSSFileInfo fileInfo = new SSSFileInfo("ssslogfile.txt");

            Console.WriteLine("\nИнформация о файле:");
            fileInfo.GetFullPath();
            fileInfo.GetFileDetails();
            fileInfo.GetFileDates();

            Console.WriteLine("\n---------------------4)-----------------------");

            SSSDirInfo dirInfo = new SSSDirInfo(Environment.CurrentDirectory);

            Console.WriteLine($"\nИнформация о директории: ");

            dirInfo.GetFileCount();
            dirInfo.GetCreationTime();
            dirInfo.GetSubdirectoryCount();
            dirInfo.GetParentDirectories();

            Console.WriteLine("\n---------------------5)-----------------------");

            SSSFileManager fileManager = new SSSFileManager();

            Console.WriteLine("\na. Работа с диском и файлами");
            fileManager.InspectDisk("C:");

            Console.WriteLine("\nb. Работа с файлами с заданным расширением");
            fileManager.ManageFiles("D:\\My_page\\УНИК\\2 курс\\1 сем\\OOP C#\\Lab12\\Lab12\\Lab12\\bin\\Debug\\fileWithTXT", ".txt");

            Console.WriteLine("\nc. Архивация и разархивация файлов");
            fileManager.ArchiveFiles("SSSInspect\\SSSFiles", "SSSFilesArchive");

            Console.ReadKey();
        }
    }
}

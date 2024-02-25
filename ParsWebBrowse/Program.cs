using System.Net; // Для работы с большим братом
using System.Text.RegularExpressions; // Для регулярных выражений

namespace ParsWebBrowse
{
    class Program
    {
        private string? url, baseDirToSave;
        private string[] urlsFiles;
        private List<string> urls = new List<string>();

        static void Main()
        {
            Program cc = new Program();
            var getUrlWhile = true;
            var getBaseDirToSave = true;

            // Получаем ссылку на страницу (это должна быть не просто строка с пробелами)

            while (getUrlWhile)
            {
                Console.Write("Введите url страницы >>> ");
                cc.setUrl(Console.ReadLine());

                if (cc.url != null && cc.url.Trim() != "")
                {
                    getUrlWhile = false;

                    while (getBaseDirToSave)
                    {
                        Console.Write("Введите полный путь до места сохранения >>> ");
                        cc.setBaseDirToSave(Console.ReadLine());

                        if (cc.baseDirToSave != null && cc.baseDirToSave.Trim() != "")
                        {
                            if ( !Directory.Exists(cc.baseDirToSave) )
                            {
                                Console.WriteLine("Папки не существует");
                                Console.WriteLine("Создаю");
                                Directory.CreateDirectory(cc.baseDirToSave);

                                Console.WriteLine("Папка создана");
                            }
                            getBaseDirToSave = false;

                            Console.WriteLine("Приступаю к скачиванию");

                            WebClient client = new WebClient();
                            
                            // url -> https://onmirtales.net/onmir-the-oddity/chapter-1/
                            // daseDir -> C:\Users\asurasuu\манга\Странность Онмир\Глава 1\Исходники

                            client.DownloadFile(cc.url, cc.baseDirToSave + "\\index.php");

                            string[] readAllLineOnFile = File.ReadAllLines(cc.baseDirToSave + "\\index.php");
                            Regex regex = new Regex(@" src=(\w*)");

                            int ouf = 0;
                            string final_string_uri;

                            foreach (string s in readAllLineOnFile)
                            {
                                // data-src=" -> это regexp, который нам нужен для онмира
                                if (regex.IsMatch(s))
                                {
                                    final_string_uri = s.Remove(0, s.IndexOf("data-src=") + 10);
                                    final_string_uri = final_string_uri.Substring(0, final_string_uri.IndexOf("\" class"));

                                    cc.urls.Add(final_string_uri);

                                    //Console.WriteLine(final_string_uri + "\n\n"); 
                                    ouf++;
                                }
                            }

                            cc.urlsFiles = cc.urls.ToArray();

                            File.Delete(cc.baseDirToSave + "\\index.php");

                            for (int i = 0; i < cc.urlsFiles.Length; i++)
                            {
                                client.DownloadFile(cc.urlsFiles[i], cc.baseDirToSave + "\\" + i + ".webp");
                            }
                        }
                        else {
                            Console.WriteLine("Введите полный путь до места сохранения");
                        }

                    }

                     
                }
                else
                {
                    Console.WriteLine("Введите url страницы");
                }
            }
        }

        public void setUrl(string? url)
        {
            this.url = url;
        }

        public void setBaseDirToSave(string? baseDirToSave)
        {
            this.baseDirToSave = baseDirToSave;
        }
    }
}
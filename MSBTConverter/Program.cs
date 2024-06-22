using System;
using System.Reflection;
using CLMS;

namespace MSBTConverter
{
    class Program
    {
        public static void Main(string[] args)
        {
            string exec_dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            //first search for any message project files
            MSBP message_project = null;
            foreach (var file in Directory.GetFiles(Path.Combine(exec_dir, "MessageProject")))
            {
                if (file.EndsWith("msbp"))
                    message_project = new MSBP(File.OpenRead(file), false);
            }

            foreach (string arg in args)
            {
                if (arg.EndsWith("msbt"))
                {
                    MSBT msbt = new MSBT(File.OpenRead(arg), false);
                    File.WriteAllText($"{arg}.yaml", msbt.ToYaml(message_project));
                }
                if (arg.EndsWith("msbp"))
                {
                    MSBP msbp = new MSBP(File.OpenRead(arg), false);
                    File.WriteAllText($"{arg}.yaml", msbp.ToYaml());
                }
                if (arg.EndsWith("yaml"))
                {
                    var dir = Path.GetDirectoryName(arg);
                    var fileName = $"{Path.GetFileNameWithoutExtension(arg)}";

                    MSBT msbt = MSBT.FromYaml(File.ReadAllText(arg), message_project);
                    File.WriteAllBytes(Path.Combine(dir, fileName), msbt.Save());
                }
            }
        }
    }
}

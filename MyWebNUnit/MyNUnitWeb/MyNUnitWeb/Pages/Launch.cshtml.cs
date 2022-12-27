using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Collections.Concurrent;
using MyNUnit;

namespace MyNUnitWeb.Pages
{
    public class HistoryModel : PageModel
    {
        [BindProperty]
        public List<IFormFile> FileUpload { get; set; } = new();
        public BlockingCollection<TestInfo> infos = new();
        
        public void OnGet(string name)
        {
        }

        public async void Upload()
        {
            foreach (var formFile in FileUpload)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = System.IO.File.Create(formFile.FileName))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
        }

        public void OnPostAsync()
        {
            Console.WriteLine("lol");
        }

        public void OnPostRun()
        {
            Upload();
            MyNUnit.MyNUnit.Run(Directory.GetCurrentDirectory());
            infos = MyNUnit.MyNUnit.typeInfos;
            Console.WriteLine("lol");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message);  
        string FilePath { get; set; }   
        string OpenFileDialog(string filter = "All files (*.*)|*.*"); 
        bool SaveFileDialog(); 
    }
}

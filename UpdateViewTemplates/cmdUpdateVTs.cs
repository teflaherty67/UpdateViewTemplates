#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace UpdateViewTemplates
{
    [Transaction(TransactionMode.Manual)]
    public class cmdUpdateVTs : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // get all the view templates in the project
            List<View> curVTs = Utils.GetAllViewTemplates(doc);

            // delete all view templates that start with a letter
            // or a number, except 17
            foreach (View curVT in curVTs)
            {
                // get the name of the view template
                string curName = curVT.Name;
                
                // check if first character is letter
                bool isLetter = !String.IsNullOrEmpty(curName) && Char.IsLetter(curName[0]);

                // check if first two charactera is number
                string firstTwo = curName.Substring(0,1);
                

                // if yes, delete it
                if (isLetter == true)
                {
                    doc.Delete(curVT.Id);
                }


            }

            // transfer the current view templates from the template file

            // assign view templates to views


            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}

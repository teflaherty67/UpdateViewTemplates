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
using System.Security.Cryptography;

#endregion

namespace UpdateViewTemplates
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
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

            // get all the views
            List<View> viewList = Utils.GetAllViews(doc);

            // get all the view templates
            List<View> vtList = Utils.GetAllViewTemplates(doc);

            // set the path for the source document
            string sourcePath = 

            
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Update View Templates");
                {
                    foreach (View curView in viewList)
                    {
                        curView.ViewTemplateId = ElementId.InvalidElementId;
                    }
                    foreach (View curView in vtList)
                    {
                        doc.Delete(curView.Id);
                    }
                }

                t.Commit();
            }
            
            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}

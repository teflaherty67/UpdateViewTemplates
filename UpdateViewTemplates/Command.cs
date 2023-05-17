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
            string sourcePath = @"S:\Shared Folders\Lifestyle USA Design\Library 2023\Template\View Templates.rvt";

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Update View Templates");
                {
                    foreach (View curView in viewList)
                    {
                        // set the assigned view template to None on all views
                        curView.ViewTemplateId = ElementId.InvalidElementId;
                    }
                    foreach (View curView in vtList)
                    {
                        // delete all view templates
                        doc.Delete(curView.Id);
                    }

                    // transfer view templates from template file
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

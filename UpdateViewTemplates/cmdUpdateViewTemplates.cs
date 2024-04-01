#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace UpdateViewTemplates
{
    [Transaction(TransactionMode.Manual)]
    public class CmdLoadGroups : IExternalCommand
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

            string revitFile = @"S:\Shared Folders\Lifestyle USA Design\Library 2023\Template\View Templates.rvt";
            
            UIDocument newUIDoc = uiapp.OpenAndActivateDocument(revitFile);
            Document newDoc = newUIDoc.Document;

            // change code to get view templates

            FilteredElementCollector colVT = new FilteredElementCollector(newDoc);
            colVT.OfCategory(BuiltInCategory.OST_Views);
            colVT.WhereElementIsElementType();

            List<ElementId> groupIdList = new List<ElementId>();
            foreach (Element curElem in colVT)
            {
                groupIdList.Add(curElem.Id);
            }

            Transform transform = null;
            CopyPasteOptions options = new CopyPasteOptions();

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Load View Templates");
                ElementTransformUtils.CopyElements(newDoc, groupIdList, doc, transform, options);
                t.Commit();
            }

            try
            {
                uiapp.OpenAndActivateDocument(doc.PathName);
                newUIDoc.SaveAndClose();
            }
            catch (Exception)
            { }

            TaskDialog.Show("Complete", "Loaded " + groupIdList.Count.ToString() + " groups into the current model.");

            return Result.Succeeded;
        }
    }
}
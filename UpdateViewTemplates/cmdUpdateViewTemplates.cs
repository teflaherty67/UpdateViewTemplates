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
    public class cmdUpdateViewTemplates : IExternalCommand
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

            // get all views by category
            
            List<View> allExtrElevs = Utils.GetAllViewsByCategory(doc, "02:Elevations");
            List<View> allRoofPlans = Utils.GetAllViewsByCategory(doc, "03:Roof Plans");
            List<View> allSections = Utils.GetAllViewsByCategory(doc, "04:Sections");
            List<View> allIntrElevs = Utils.GetAllViewsByCategory(doc, "05:Interior Elevations");
            List<View> allElectrical = Utils.GetAllViewsByCategory(doc, "06:Electrical Plans");

            // get all views by category and view name contains

            List<View> allAnnoPlans = Utils.GetAllViewsByCategoryAndNameContains(doc, "01:Floor Plans", "Annotation");
            List<View> allDimPlans = Utils.GetAllViewsByCategoryAndNameContains(doc, "01:Floor Plans", "Dimension");

            // get all views by category name contains

            List<View> allFormPlans = Utils.GetAllViewsByCategoryNameContains(doc, "Form/Foundation Plan");


            // assign view templates by category to views

            // delete all unused view templates

            // update view template names if needed


            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}

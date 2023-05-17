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

            List<View> viewList = Utils.GetAllViews(doc);

            List<View> vtList = Utils.GetAllViewTemplates(doc);

            List<string> vtNames = Utils.GetAllViewTemplateNames(doc);
            vtNames.Sort();

            List<string> vtNameList = new List<string> { "15", "14", "13", "12", "11", "10", "09", "08", "07" };

            string vtForm = "00-Form/Foundation Plan";

            string vtFrmgElev = "16-Framing Elevation";

            // if the project contains View Template named 00-Form/Foundation Plan

            // then increase number by 1 for 07 - 15 & change 16 to 18 & 00 to 07

            // update the Category parameter to match

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Update View Templates");
                {
                    if (vtNames.Contains(vtForm))
                    {
                        foreach (View curView in vtList)
                        {
                            if (curView.Name == vtFrmgElev)
                            {
                                curView.Name = curView.Name.Replace(vtFrmgElev, "18-Framing Elevation");

                                Parameter catParam = null;

                                string curCat = "";

                                foreach (Parameter curParam in curView.Parameters)
                                {
                                    if (curParam.Definition.Name == "Category")
                                    {
                                        catParam = curParam;

                                        curCat = curParam.AsString();
                                    }
                                }

                                // change the value of the caegory parameter, replace '-' with ':'
                            }

                            // then loop through vtList looking for names that contain
                            // strings in vtNamesList and increment them by 1 number

                            // then change 00-Form/Foundation Plan to 07-Form/Foundation Plan
                        }

                        // then loop through vt
                    }

                    //foreach (View curView in vtList)
                    //{
                    //    if (curView.Name == vtForm)
                    //    {
                    //        curView.Name = curView.Name.Replace(vtForm, "07-Form/Foundation Plan");
                    //    }

                    //    if (curView.Name == vtFrmgElev)
                    //    {
                    //        curView.Name = curView.Name.Replace(vtFrmgElev, "18-Framing Elevation");
                    //    }
                    //}
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

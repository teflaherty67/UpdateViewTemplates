using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateViewTemplates
{
    internal static class Utils
    {
        internal static RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            RibbonPanel currentPanel = GetRibbonPanelByName(app, tabName, panelName);

            if (currentPanel == null)
                currentPanel = app.CreateRibbonPanel(tabName, panelName);

            return currentPanel;
        }        

        internal static RibbonPanel GetRibbonPanelByName(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel tmpPanel in app.GetRibbonPanels(tabName))
            {
                if (tmpPanel.Name == panelName)
                    return tmpPanel;
            }

            return null;
        }

        internal static List<View> GetAllViews(Document curDoc)
        {
            FilteredElementCollector m_colviews = new FilteredElementCollector(curDoc);
            m_colviews.OfCategory(BuiltInCategory.OST_Views);

            List<View> m_views = new List<View>();
            foreach (View x in m_colviews.ToElements())
            {
                m_views.Add(x);
            }

            return m_views;
        }

        #region View Templates
        public static List<View> GetAllViewTemplates(Document curDoc)
        {
            List<View> returnList = new List<View>();
            List<View> viewList = GetAllViews(curDoc);

            //loop through views and check if is view template
            foreach (View v in viewList)
            {
                if (v.IsTemplate == true)
                {
                    //add view template to list
                    returnList.Add(v);
                }
            }

            return returnList;
        }

        public static List<string> GetAllViewTemplateNames(Document m_doc)
        {
            //returns list of view templates
            List<string> viewTempList = new List<string>();
            List<View> viewList = new List<View>();
            viewList = GetAllViews(m_doc);

            //loop through views and check if is view template
            foreach (View v in viewList)
            {
                if (v.IsTemplate == true)
                {
                    //add view template to list
                    viewTempList.Add(v.Name);
                }
            }

            return viewTempList;
        }

        public static View GetViewTemplateByName(Document curDoc, string viewTemplateName)
        {
            List<View> viewTemplateList = GetAllViewTemplates(curDoc);

            foreach (View v in viewTemplateList)
            {
                if (v.Name == viewTemplateName)
                {
                    return v;
                }
            }

            return null;
        }

        #endregion

        internal static Parameter GetParameterByName(Element curElem, string paramName)
        {
            foreach (Parameter curParam in curElem.Parameters)
            {
                if (curParam.Definition.Name.ToString() == paramName)
                    return curParam;
            }

            return null;
        }

        internal static void SetParameterByName(Element element, string paramName, string value)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);

            if (paramList != null)
            {
                Parameter param = paramList[0];

                param.Set(value);
            }
        }

        internal static void SetParameterByName(Element element, string paramName, int value)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);

            if (paramList != null)
            {
                Parameter param = paramList[0];

                param.Set(value);
            }
        }

        #region Categories
        public static List<String> GetAllCategories(Document curDoc)
        {
            List<String> catList = new List<String>();

            //loop through categories in current model file
            foreach (Category curCat in curDoc.Settings.Categories)
            {
                //add to list
                catList.Add(curCat.Name);
            }
            //sort list
            catList.Sort();
            return catList;
        }

        internal static List<View> GetAllViewsByCategory(Document doc, string v)
        {
            throw new NotImplementedException();
        }

        internal static List<View> GetAllViewsByCategoryAndNameContains(Document doc, string v1, string v2)
        {
            throw new NotImplementedException();
        }

        internal static List<View> GetAllViewsByCategoryNameContains(Document doc, string v)
        {
            throw new NotImplementedException();
        }
        #endregion

        internal static void LoadViewTemplates(UIApplication uiapp, Document doc, string fileName)
        {        
            UIDocument newUIDoc = uiapp.OpenAndActivateDocument(fileName);
            Document newDoc = newUIDoc.Document;

            // change code to get view templates

            // get all the view templates
            List<View> vtList = Utils.GetAllViewTemplates(newDoc);

            List<ElementId> groupIdList = new List<ElementId>();
            foreach (Element curElem in vtList)
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
                newDoc.Close(false);
            }
            catch (Exception)
            { }

            TaskDialog.Show("Complete", "Loaded " + groupIdList.Count.ToString() + " groups into the current model.");
        }
    }
}

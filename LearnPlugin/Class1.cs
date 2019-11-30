using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace LearnPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    public class Class1 : IExternalCommand
        {
            public Result Execute(ExternalCommandData commandData,ref string message, ElementSet elements)
        {
    
            //Get application and documnet objects

            UIApplication uiapp = commandData.Application;

            Document doc = uiapp.ActiveUIDocument.Document;

            try
            {
                //Define a reference Object to accept the pick result

                Reference pickedref = null;

                //Pick a group

                Selection sel = uiapp.ActiveUIDocument.Selection;

                GroupPickFilter selFilter = new GroupPickFilter();
                pickedref = sel.PickObject(ObjectType.Element, selFilter, "Please Select a group");
                //pickedref = sel.PickObject(ObjectType.Element, "Please select a group");

                Element elem = doc.GetElement(pickedref);

                Group group = elem as Group;

                //Pick point

                XYZ point = sel.PickPoint("Please pick a point to place group");

                //Place the group

                Transaction trans = new Transaction(doc);

                trans.Start("Lab");

                doc.Create.PlaceGroup(point, group.GroupType);

                trans.Commit();
            }

            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
    public class GroupPickFilter: ISelectionFilter
    {
        public bool AllowElement(Element e)
        {
            return (e.Category.Id.IntegerValue.Equals((int)BuiltInCategory.OST_IOSModelGroups));
        }
        public bool AllowReference(Reference r, XYZ p)
        {
            return false;
        }

    }
}

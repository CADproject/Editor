using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CADController
{
    using ObjectId = System.UInt32;
    using DocumentId = System.UInt32;
       
    class Program
    {
        static void Main()
        {
            IntPtr curSession = CoreWrapper.sessionFactory();

            IntPtr pDoc1 = CoreWrapper.documentFactory();
            DocumentId docID1 = CoreWrapper.attachDocument(curSession, pDoc1);

            IntPtr pDoc2 = CoreWrapper.documentFactory();
            DocumentId docID2 = CoreWrapper.attachDocument(curSession, pDoc2);

            ObjectId p1 = Operations.createPoint(curSession, docID1, 5, 10);
            ObjectId l1 = Operations.createLine(curSession, docID1, 1, 2, 3, 4);
            ObjectId c1 = Operations.createCircle(curSession, docID1, 2, 6, 8, 1);

            ObjectId line1 = Operations.createLine(curSession, docID1, 1, 2, 3, 4);
            ObjectId line2 = Operations.createLine(curSession, docID1, 5, 6, 7, 8);
            ObjectId line3 = Operations.createLine(curSession, docID1, 9, 10, 11, 12);
            /*
            std::vector<ObjectId> objects;
            objects.push_back(line1);
            objects.push_back(line2);
            objects.push_back(line3);
            ObjectId con1 = Operations.createContour(curSession, docID1, objects);
            */
            Operations.display(curSession, docID1);

            Operations.deleteObject(curSession, docID1, p1);
            //Operations.deleteObject(curSession, docID1, con1);
            Operations.display(curSession, docID1);

            Operations.undo(curSession, docID1);
            Operations.display(curSession, docID1);

            ObjectId c2 = Operations.createCircle(curSession, docID1, 7, 7, 9, 9);
            Operations.display(curSession, docID1);

            Operations.redo(curSession, docID1);
            Operations.display(curSession, docID1);

            Console.ReadKey();
        }
    }
}

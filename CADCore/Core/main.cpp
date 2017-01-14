#include <iostream>
#include "session.h"
#include "document.h"
#include "controller.h"

int main()
{
	Session curSession;
	Document* pDoc1 = new Document();
	DOCID docID1 = curSession.attachDocument(pDoc1);

	Document* pDoc2 = new Document();
	DOCID docID2 = curSession.attachDocument(pDoc2);
	
	OBJID p1 = createPoint(curSession, docID1, 5, 10);
	OBJID l1 = createLine(curSession, docID1, 1, 2, 3, 4);
	OBJID c1 = createCircle(curSession, docID1, 2, 6, 8, 1);
	
	OBJID line1 = createLine(curSession, docID1, 1, 2, 3, 4);
	OBJID line2 = createLine(curSession, docID1, 5, 6, 7, 8);
	OBJID line3 = createLine(curSession, docID1, 9, 10, 11, 12);
		
	std::vector<OBJID> objects;
	objects.push_back(line1);
	objects.push_back(line2);
	objects.push_back(line3);	
	OBJID con1 = createContour(curSession, docID1, objects);
	display(curSession, docID1);
	
	//destroyContour(curSession, docID1, con1);
	deleteObject(curSession, docID1, p1);
	deleteObject(curSession, docID1, con1);
	display(curSession, docID1);

	undo(curSession, docID1);
	display(curSession, docID1);

	OBJID c2 = createCircle(curSession, docID1, 7, 7, 9, 9);
	display(curSession, docID1);

	redo(curSession, docID1);
	display(curSession, docID1);
	
	system("pause");
	return 0;
}
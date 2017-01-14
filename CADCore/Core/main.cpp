#include <iostream>
#include "session.h"
#include "document.h"
#include "controller.h"

int main()
{
	Session curSession;
	Document* pDoc1 = new Document();
	DOCID docID1 = curSession.attachDocument(pDoc1);
	
	OBJID p1 = createPoint(curSession, docID1, 5, 10);
	display(curSession, docID1);

	undo(curSession, docID1);
	undo(curSession, docID1);
	undo(curSession, docID1);
	display(curSession, docID1);
	
	/*
	OBJID cl1 = createLine(curSession, docID1, 0, 0, 5, 0);
	OBJID cl2 = createLine(curSession, docID1, 5, 0, 5, 8);
	OBJID cl3 = createLine(curSession, docID1, 5, 8, 0, 8);
	OBJID cl4 = createLine(curSession, docID1, 0, 8, 0, 0);
		
	OBJID temp = createCircle(curSession, docID1, 4, 2, 5, 1);

	display(curSession, docID1);

	std::vector<OBJID> objects;
	objects.push_back(cl1);
	objects.push_back(cl2);
	objects.push_back(cl3);
	objects.push_back(cl4);

	OBJID con1 = createContour(curSession, docID1, objects);
	
	deleteObject(curSession, docID1, temp);

	display(curSession, docID1);

	destroyContour(curSession, docID1, con1);

	display(curSession, docID1);
<<<<<<< HEAD
	
=======
	*/
>>>>>>> fixed undo/redo bug
	system("pause");
	return 0;
}
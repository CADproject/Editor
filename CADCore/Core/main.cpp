#include <iostream>
#include "session.h"
#include "document.h"
#include "controller.h"

int main()
{
	Session curSession;
	Document doc1, doc2;

	DOCID docID1 = curSession.attachDocument(&doc1);
	//DOCID docID2 = curSession.attachDocument(&doc2);

	OBJID p1 = createPoint(curSession, docID1, 5, 10);
	//OBJID l1 = createLine(curSession, docID1, 1, 2, 5, 12);
	//OBJID c1 = createCircle(curSession, docID1, 7, 7, 10, 7);

	//OBJID p2 = createPoint(curSession, docID2, 1, 1);
	//OBJID l2 = createLine(curSession, docID2, 3, 4, 6, 8);
	//OBJID c2 = createCircle(curSession, docID2, 3, 3, 3, 5);

	//display(curSession, docID1);
	//display(curSession, docID2);

	system("pause");
	return 0;
}
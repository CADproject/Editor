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
		
	system("pause");
	return 0;
}
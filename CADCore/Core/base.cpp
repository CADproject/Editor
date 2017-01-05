#include "base.h"

void Base::commit(void)
{
	_undoredo.commit(_base);
}

void Base::undo(void)
{
	_undoredo.undo(_base);
}

void Base::redo(void)
{
	_undoredo.redo(_base);
}
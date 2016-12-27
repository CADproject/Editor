#include "headers.h"
#include "model_tree.h"
using namespace std;

unsigned ModelTree::_counter;

Feature* Node::createFeature(void)
{
	Feature* temp = nullptr;

	switch(_featureId)
	{
	case POINT_FEATURE:
		temp = new createPointFeat;
		break;
	case LINE_FEATURE:
		temp = new createLineFeat;
		break;
	case CIRCLE_FEATURE:
		temp = new createCircleFeat;
		break;
	case ARC_FEATURE:
		temp = new createArcFeat;
		break;
	case POLYLINE_FEATURE:
		temp = new createPolylineFeat;
		break;
	case SET_LINE_POINT_FEATURE:
		temp = new setLinePointFeat;
		break;
	case MESH_FEATURE:
		temp = new createMeshFeat;
		break;
	case SUPPORT_FEATURE:
		temp = new createSupportFeat;
		break;
	case LOAD_FEATURE:
		temp = new createLoadFeat;
		break;
	default:
		assert(false);
		break;
	}
	return temp;
}

bool Node::runFeature(Base& base)
{
	Feature* temp = createFeature();
	bool res = temp->runFeat(_source, _result, base);	
	delete temp;
	return res;		
}

void ModelTree::runTree(Node* pNode)
{
	bool res = pNode->runFeature(_base);
	if(!res) return;

	vector<Node*> nodeChildren = pNode->getChildren();
	
	if( !nodeChildren.empty() )
	{
		for(size_t i = 0; i < nodeChildren.size(); i++)
			runTree( nodeChildren.at(i) );
	}
}

void ModelTree::addNode(ConnectData& source, FEATID featureID)
{
	//в дальнейшем надо будет узнавать сколько предполагается создать объектов
	//пока предполагаем, что один
	ConnectData result;
	++_counter;
	OBJID newID = _counter;
	vector<OBJID> ids;
	ids.push_back(newID);
	result.setIDs(ids);

	//создаем новый узел
	Node* newNode = new Node(source, featureID, result);
	
	//производим поиск места вставки и добавляем
	//мест может быть несколько, т.к. это фактически однонаправленный ациклический граф, а не дерево
	for(size_t i = 0; i < source.getIDs().size(); i++)
	{
		Node* foundNode = searchNode( _root, source.getIDs().at(i) );
		foundNode->AddChildren(newNode);
		newNode->runFeature(_base);
	}
}

void ModelTree::deleteNode(Node* pNode)
{
	vector<Node*> nodeChildren = pNode->getChildren();
	
	if( !nodeChildren.empty() )
	{
		for(size_t i = 0; i < nodeChildren.size(); i++)
			deleteNode( nodeChildren.at(i) );
	}
	
	vector<OBJID> resultObjects = pNode->getResultIDs();
	
	for(size_t i = 0; i < resultObjects.size(); i++)
		_base.detachObject( resultObjects.at(i) );
	
	delete pNode;
	
	for(size_t i = 0; i < pNode->getParents().size(); i++)
	{
		vector<Node*> brothers = pNode->getParents().at(i)->getChildren();
		auto iter = find(brothers.begin(), brothers.end(), pNode);
		brothers.erase(iter);
	}
}

Node* ModelTree::searchNode(Node* startNode, OBJID id)
{
	static bool found = false;

	if(!found)
	{
		vector<OBJID> nodeResult = startNode->getResultIDs();
	
		for(size_t i = 0; i < nodeResult.size(); i++)
		{
			if(id == nodeResult.at(i))
			{
				found = true;
				return startNode;
			}
		}

		vector<Node*> nodeChildren = startNode->getChildren();
		
		if(!nodeChildren.empty())
		{
			for(size_t i = 0; i < nodeChildren.size(); i++)
				searchNode(nodeChildren.at(i), id);
		}
		else return nullptr;
	}
	else
		return startNode;
}
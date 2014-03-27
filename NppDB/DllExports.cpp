
#include "stdafx.h"

#include "PluginWrapper.h"


extern "C" __declspec(dllexport) 
bool __cdecl isUnicode()
{
	//return NppDB::NPPExports::isUnicode();
	return NppDB::PluginWrapper::isUnicode();
}

extern "C" __declspec(dllexport)
void __cdecl setInfo(NppData1 notepadPlusData)
{
	//IntPtr i = IntPtr((void *)(&notepadPlusData));
	//NppDB::NPPExports::setInfo(static_cast<NppDB::NppData> (Marshal::PtrToStructure(IntPtr((void *)(&notepadPlusData)), NppDB::NppData::typeid)));
	NppDB::PluginWrapper::setInfo(notepadPlusData);
}
	
extern "C" __declspec(dllexport) 
void * __cdecl getFuncsArray(int *nbF)
{
	//return NppDB::NPPExports::getFuncsArray(*nbF).ToPointer();
	return NppDB::PluginWrapper::getFuncsArray(nbF);
}

extern "C" __declspec(dllexport) 
LRESULT __cdecl messageProc(UINT Message, WPARAM wParam, LPARAM lParam)
{
	return NppDB::PluginWrapper::messageProc(Message,wParam,lParam);
}

extern "C" __declspec(dllexport)
const char * __cdecl getName()
{
	//return static_cast<char *>(NppDB::NPPExports::getName().ToPointer());
	return NppDB::PluginWrapper::getName();
}
extern "C" __declspec(dllexport)
void __cdecl beNotified(void * notifyCode)
{
	//NppDB::NPPExports::beNotified(static_cast<NppSQLExec::SCNotification>(Marshal::PtrToStructure(IntPtr(notifyCode), NppDB::SCNotification::typeid)));
	NppDB::PluginWrapper::beNotified(notifyCode);
}
	
#include "stdafx.h"

#include "PluginWrapper.h"



void NppDB::PluginWrapper::checkType()
{
	if(_typ != nullptr) return;
	String^ path = Path::Combine( Path::GetDirectoryName (Uri::UnescapeDataString((gcnew Uri(Assembly::GetExecutingAssembly()->CodeBase))->AbsolutePath)), _pluginName ,_assemblyName);
	Assembly^ assem = Assembly::LoadFrom(path);
	_typ =  assem->GetType(L"NppDB.NppDBPlugin");
	_pluginObj = Activator::CreateInstance(_typ);
	
}

void NppDB::PluginWrapper::checkTypeNppData()
{
	if(_typNppData != nullptr) return;
	String^ path = Path::Combine( Path::GetDirectoryName (Uri::UnescapeDataString((gcnew Uri(Assembly::GetExecutingAssembly()->CodeBase))->AbsolutePath)), _pluginName ,_assemblyName);
	Assembly^ assem = Assembly::LoadFrom(path);
	_typNppData =  assem->GetType(L"NppDB.NppData");
}

void NppDB::PluginWrapper::checkSCNotification()
{
	if(_typSCNotification != nullptr) return;
	String^ path = Path::Combine( Path::GetDirectoryName (Uri::UnescapeDataString((gcnew Uri(Assembly::GetExecutingAssembly()->CodeBase))->AbsolutePath)), _pluginName ,_assemblyName);
	Assembly^ assem = Assembly::LoadFrom(path);
	_typSCNotification =  assem->GetType(L"NppDB.SCNotification");
}

bool NppDB::PluginWrapper::isUnicode()
{
	checkType();
	return static_cast<bool>( _typ->GetMethod(L"isUnicode")->Invoke(_pluginObj, nullptr));
}

void  NppDB::PluginWrapper::setInfo(NppData1 notepadPlusData)
{
	checkType();
	checkTypeNppData();
	_typ->GetMethod(L"setInfo")->Invoke(_pluginObj, gcnew array<Object^> { Marshal::PtrToStructure(IntPtr((void *)(&notepadPlusData)), _typNppData) });
}

void * NppDB::PluginWrapper::getFuncsArray(int *nbF)
{
	checkType();
	array<Object^>^ params = gcnew array<Object^> {*nbF};
	void * ret = static_cast<void *>(( static_cast<IntPtr>(_typ->GetMethod(L"getFuncsArray")->Invoke(_pluginObj, params ))).ToPointer());
	*nbF =  static_cast<int>(params[0]);
	return ret;
}
		
bool NppDB::PluginWrapper::messageProc(UINT Message, WPARAM wParam, LPARAM lParam)
{
	return static_cast<bool>(_typ->GetMethod(L"messageProc")->Invoke(_pluginObj, gcnew array<Object^> { Message, UIntPtr(wParam), IntPtr(lParam)}));
}
		
const char * NppDB::PluginWrapper::getName()
{
	checkType();
	return static_cast<char *>(( static_cast<IntPtr>( _typ->GetMethod(L"getName")->Invoke(_pluginObj, nullptr ))).ToPointer());
}

void NppDB::PluginWrapper::beNotified(void * notifyCode)
{
	checkType();
	checkSCNotification();
	_typ->GetMethod(L"beNotified")->Invoke(_pluginObj, gcnew array<Object^> { Marshal::PtrToStructure(IntPtr(notifyCode), _typSCNotification) });
}

NppDB::PluginWrapper^ NppDB::PluginWrapper::Instance()
{
	if(_inst == nullptr) _inst = gcnew NppDB::PluginWrapper();
	return _inst;
}
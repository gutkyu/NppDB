// PluginLoader.h

#include <windows.h>
#pragma unmanaged
typedef	struct NppData1 {
		HWND _nppHandle;
		HWND _scintillaMainHandle;
		HWND _scintillaSecondHandle;
	};

#pragma once

using namespace System;
using namespace System::IO;
using namespace System::Reflection;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace NppDB {
	
#pragma managed
	/*
	Dictionary<String^,MethodInfo^>^ _miSet = gcnew Dictionary<String^,MethodInfo^>(6);
	String^ _asmPath = L".\\NppDB\\NppDB.dll";
	String^ _typeName = L"NPPExports";

	void checkExportMethod(String^ methodName)
	{
		 Assembly^ asm = Assembly::LoadFrom(_asmPath);
		 _miSet[methodName] = asm->GetType(_typeName)->GetMethod(methodName);
	}

	void CallMethod(String^ name)
	{

	}
	*/
	
	/*
	extern "C" __declspec(dllexport)  class npwrapper
	{
	private:

	public :
		static  void __cdecl test()
		{
			NppDB::NPPExports::isUnicode();
		}
	};
	*/

	
	
	ref class PluginWrapper
	{
	private:
		static Type^ _typ = nullptr;
		static Type^ _typNppData = nullptr;
		static Type^ _typSCNotification = nullptr;
		static String^ _pluginName = "NppDB";
		static String^ _assemblyName = L"NppDB.Plugin.dll";
		static void checkType(void);
		/*
		{
			if(_typ == nullptr)
			{
				String^ path = Path::Combine( Path::GetDirectoryName ((gcnew System::Uri(Assembly::GetExecutingAssembly()->CodeBase))->AbsolutePath), _pluginName ,_assemblyName);
				Assembly^ assem = Assembly::LoadFrom(path);
				_typ =  assem->GetType(L"NppDB.NPPExports");
			}
		}
		*/
		static void checkTypeNppData(void);
		/*{
			if(_typNppData == nullptr)
			{
				String^ path = Path::Combine( Path::GetDirectoryName ((gcnew System::Uri(Assembly::GetExecutingAssembly()->CodeBase))->AbsolutePath), _pluginName ,_assemblyName);
				Assembly^ assem = Assembly::LoadFrom(path);
				_typNppData =  assem->GetType(L"NppDB.NppData");
			}
		}*/

		static void checkSCNotification(void);
		/*{
			if(_typSCNotification == nullptr)
			{
				String^ path = Path::Combine( Path::GetDirectoryName ((gcnew System::Uri(Assembly::GetExecutingAssembly()->CodeBase))->AbsolutePath), _pluginName ,_assemblyName);
				Assembly^ assem = Assembly::LoadFrom(path);
				_typSCNotification =  assem->GetType(L"NppDB.SCNotification");
			}
		}
		*/
	public:
		static bool isUnicode(void);
		/*{
			 checkType();
			 return static_cast<bool>( _typ->GetMethod(L"isUnicode")->Invoke(nullptr, nullptr));
		}
		*/
		static void  setInfo(NppData1 notepadPlusData);
		/*{
			checkType();
			checkTypeNppData();
			 _typ->GetMethod(L"setInfo")->Invoke(nullptr, gcnew array<Object^> { Marshal::PtrToStructure(IntPtr((void *)(&notepadPlusData)), _typNppData) });
		}*/

		static void * getFuncsArray(int *nbF);
		/*{
			checkType();
			array<Object^>^ params = gcnew array<Object^> {*nbF};
			void * ret = static_cast<void *>(( static_cast<IntPtr>(  _typ->GetMethod(L"getFuncsArray")->Invoke(nullptr, params ))).ToPointer());
			*nbF =  static_cast<int>(params[0]);
			return ret;
		}*/
		
		static bool messageProc(UINT Message, WPARAM wParam, LPARAM lParam);
		/*{
			 return true;
		}
		*/
		static const char * getName(void);
		/*{
			checkType();
			return static_cast<char *>(( static_cast<IntPtr>( _typ->GetMethod(L"getName")->Invoke(nullptr, nullptr ))).ToPointer());
		}
		*/
		static void beNotified(void * notifyCode);
		/*{
			checkType();
			checkSCNotification();
			_typ->GetMethod(L"beNotified")->Invoke(nullptr, gcnew array<Object^> { Marshal::PtrToStructure(IntPtr(notifyCode), _typSCNotification) });
		}*/
	};
	

}


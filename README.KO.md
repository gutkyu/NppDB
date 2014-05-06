# NppDB
NppDB는 다양한 Database에 접속해서 SQL 구문를 실행하고 그 결과를 확인할 수 있는 Notepad++ Plugin이다.

## 요구사항
* .Net Framwork 4.0 설치 필요
* Notepad++ 6.5 (unicode)

## 지원가능한 Databases
MS SQL Server ( 2008 R2에서 작동확인), SQLite

## 구성
1. Database Connect Manager 
	Database 등록, 삭제, 연결, 끊기
	Database 하위 구성요소들의 관계를 계층적으로 보여준다.
	SQL 구문 실행환경을 만든다.
2. SQL Result
	SQL Query의 실행 결과를 보여준다.
3. Document
	SQL 구문을 작성하는 공간
	실행할 SQL구문은 블럭으로 선택해야 한다. 

## 설치
1. Binary File들의 zip파일을 임시 디렉토리에 다운로드하고 압축을 해제한다.
2. NppDB.dll 파일과 nppdb 디렉토리를 Notepad++의 plugins 디렉토리 아래로 복사한다.


## Database에 SQL 구문을 보내고 결과를 확인하는 방법
1. Database Connect Manager에서 Database 연결한다.
2. 최상위에 위치한 Database Connect Node를 Double Click해서 하위 Database Node들을 확장한다.
3. SQL Query의 대상 Database Node를 선택한 후 오른쪽 버튼을 눌러서 불러낸 팝업창에서 ‘Open’ 메뉴을 선택하면 새로운 Document와 SQL Result가 생성된다.
4. Document에 SQL Query를 작성한다.
5. 그 sql 구문을 블럭으로 선택한다.
6. ‘Plugins/NppDB/Execute SQL’을 실행한다. (단축키 F9)

## 사용 방법
Database Connect Manager 열기
Notepad++에서 메뉴를 선택하거나 Toolbar에서 icon 선택

### 새로운 Database 서버 등록
1. Database Connect Manager의 Toolbar에서 icon 누른다.
2. 연결할 Database를 선택한다.
3. 선택한 Database에 적합한 연결과정을 거치면 새로운 Database Connect Node가 등록된다.
	참고) 각 Database의 모듈별로 서로 다른 연결과정을 제공한다. 
    
### 하위 요소 확인
하위 요소의 상세 내용을 확인하려면 그 요소에 해당하는 Node를 선택한 다음 더블 클릭한다.
그 Node의 하위 요소 역시 또 다른 계층 구조를 이루고 있기 때문에 같은 방법으로 그 구조를 확인할 수 있다.

### SQL 구문 실행 환경 만들기
NppDB에서 SQL 구문을 작성하고 확인하려면 1) Database Connect Manager에서 해당 Database연결을 생성하고  2) SQL 구문 작성을 위한 새로운 Document를 추가한 다음 3)그 실행 결과를 보여주는 SQL Result 영역을  Notepadd++에 표시해야한다.
NppDB는 번거롭게 모든 과정은 각각 별도로 실행하지 않고 아래에 소개하는 두가지 방법으로 간단하게 실행 환경을 제공한다.

1. Database Node의 팝업 메뉴에서 ‘Open’ 선택한다.
	이 메뉴는 SQL 구문을 작성할 수 있는 빈 문서와 SQL Result 영역을 생성한다.
	
2. Table Node의 팝업 메뉴에서 ‘Select … Top 100’나 ‘Select … Limit 100’처럼 SQL 구문의 등록한 메뉴를 실행한다.
	이 메뉴는 Table과 연관된 SQL 구문을 새로 생성한 문서에 보낸 다음 그 구문의 실행 결과를 SQL Result에 보이도록 일련의 과정을 실행한다.
	선택한 후 오른쪽 마우스 버튼을 누른 다음 나타난 팝업창에서 Open 메뉴를 선택하거나 그 Node를 더블 클릭한다.

###SQL 구문 실행
1. 작성할 문서가 실행 환경에 있는지 확인한다.(sql result가 아래에 있는지 확인)
2. 문서 안에 SQL 구문을 작성하고 그 구문을 블럭으로 선택한다. 
3. Execute SQL (F9)를 실행하면 블럭으로 선택한 sql 구문의 결과가 Document 아래에 있는 SQL Result에 나타난다.


##향후 계획
* DataTable Display과정 개선
* Database 모듈 버전 표시
* MS SQL 세션ID 표시

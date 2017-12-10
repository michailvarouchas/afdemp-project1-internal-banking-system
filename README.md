# Project-1-Csharp-Streams
## 1. Internal Bank Accounts Class
### Properties:
* Username
* TransactionDate
* Amount
#### Methods:
* ToString() => override
* AccountsHeader()
## 2. Database Access Class
### Fields:
* _connectionCredentials
### Methods:
* ViewAllAccouns()
* ViewSingleAccount(username, password)
* IsMoneyTransfered(sen, rec, amount, out errMsg
* IsOnDB(username)
* IsOnDB(username, password) => overload
## 3. File’s Access Class
### Methods:
* SendTodayStatement(list, username)

Takes a list of strings and a username and writes on a txt file. Each list item describes one user action and spans an entire row on the txt file “statement_username_date.txt”.

## 4. Application’s Menus
### Fields:
* _superAdminHeader 
* _superAdminMenu 
* _simleUserHeader
* _simpleUserMenu

### Methods:
* DisplaySelectionMenu(userLevel)

Displays a menu on the console depending on user level.

## 5. Login Screen
### Methods:
* WelcomeScreen()
* ReadPassword()
* IsMoney()

## 6. Main Program
* User Login (3 tries)
* Proper Menu Display
* Allow user to select the desired action from the menu.
* Once action is performed (or not) user returns back to the menu by pressing any key.
* Program terminates when user selects exit or send today’s statement from the menu.


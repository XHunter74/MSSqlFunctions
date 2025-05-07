# Step 1: Enable CLR support in SQL Server

```sql
sp_configure 'clr enabled', 1;
RECONFIGURE;
```

# Step 2: Create a C# Project

Open Visual Studio.

Create a new project:

- Type: Class Library (.NET Framework) (e.g., .NET Framework 4.8)  
  **Note:** .NET Core and Netstandard libraries are not compatible with MS SQL CLR.

Name the project, for example: `SqlIsPalindrome`

# Step 3: Install the System.Data.SqlClient NuGet Package

```powershell
Install-Package System.Data.SqlClient
```

# Step 4: Write the Function

```csharp
using Microsoft.SqlServer.Server;

namespace SqlIsPalindrome
{
    public class SqlFunctions
    {
        [SqlFunction]
        public static bool IsPalindrome(string input)
        {
            int left = 0;
            int right = input.Length - 1;

            while (left < right)
            {
                if (input[left] != input[right])
                {
                    return false;
                }

                left++;
                right--;
            }

            return true;
        }
    }
}
```

⚠️ Be sure to add the `[SqlFunction]` attribute.

# Step 5: Build the Project

Build → Build Solution (you will get a `.dll` file)

# Step 6: Register the Assembly in SQL Server

Allow UNSAFE permissions if needed:

```sql
ALTER DATABASE [YourDatabaseName] SET TRUSTWORTHY ON;
```

Create the assembly:

```sql
CREATE ASSEMBLY SqlIsPalindrome
FROM 'C:\Path\To\SqlIsPalindrome.dll'
WITH PERMISSION_SET = SAFE; -- or UNSAFE if necessary
```

> **Note:** Use a path accessible from MS SQL. If you are using MS SQL with Docker, it is better to place the assembly in the container or a mapped volume.

# Step 7: Create the SQL Function

```sql
CREATE FUNCTION dbo.IsPalindrome(@value NVARCHAR(MAX))
RETURNS BIT
AS EXTERNAL NAME SqlIsPalindrome.[SqlIsPalindrome.SqlFunctions].IsPalindrome;
```

# Step 8: Use the Function

```sql
SELECT 'level' AS text, dbo.IsPalindrome('level') AS isPalindrome
UNION ALL 
SELECT 'function' AS text, dbo.IsPalindrome('function') AS isPalindrome;
```
Result:  
![image](https://github.com/user-attachments/assets/e8abf45a-2114-4876-9c7a-f444311092a8)

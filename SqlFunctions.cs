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

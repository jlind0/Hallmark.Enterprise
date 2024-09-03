using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace HallData.Utilities
{
	public static class ExpressionExtensions
	{
		public static MemberExpression GetMemberExpression(this Expression expression)
		{
			if (expression is MemberExpression)
			{
				return (MemberExpression)expression;
			}
			else if (expression is LambdaExpression)
			{
				var lambdaExpression = expression as LambdaExpression;
				if (lambdaExpression.Body is MemberExpression)
				{
					return (MemberExpression)lambdaExpression.Body;
				}
				else if (lambdaExpression.Body is UnaryExpression)
				{
					return ((MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
				}
			}
			return null;
		}

		public static string GetPropertyPath(this Expression expr)
		{
			List<string> path = new List<string>();
			MemberExpression memberExpression = expr.GetMemberExpression();
			while (memberExpression != null)
			{
				path.Add(memberExpression.Member.Name);
				memberExpression = memberExpression.Expression.GetMemberExpression();
			}
			if (path.Count == 0)
				return null;
			path.Reverse();
			return string.Join(".", path);
		}
	}
}

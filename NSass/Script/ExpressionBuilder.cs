using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSass.Tree;
using NSass.Tree.Expressions;

namespace NSass.Script
{
    public static class ExpressionBuilder
    {
        public static PropertyNode Push(PropertyNode node, Token token)
        {
            if (node.Children.Count == 0)
            {
                node.Children.Add(CreateNode(token, node));
                return node;
            }

            PushOn((ExpressionNode)node.Children.First(), token);
            return node;
        }

        private static void PushOn(ExpressionNode node, Token token)
        {
            if (node.IsOperator)
            {
                if (IsOperator(token))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    PushSymbolOnOperator(node, token);
                }
            }
            else
            {
                if (IsOperator(token))
                {
                    PushOperatorOnSymbol(node, token);
                }
                else
                {
                    throw new SyntaxException("Operator expected before: " + token.Value);
                }
            }
        }

        private static void PushSymbolOnOperator(ExpressionNode existingOperator, Token token)
        {
            if (existingOperator.Children.Count < 2)
            {
                existingOperator.Children.Add(CreateNode(token, existingOperator));
            }
            else
            {
                PushOn((ExpressionNode)existingOperator.Children.Last(), token);
            }
        }

        private static void PushOperatorOnSymbol(ExpressionNode existingSymbol, Token token)
        {
            // The replacement should take the place of the existingSymbol, and the existingSymbol
            // should be a child of the replacement.
            ExpressionNode replacement = CreateNode(token, existingSymbol.Parent);
            existingSymbol.Parent.ReplaceChild(existingSymbol, replacement);
            existingSymbol.Parent = replacement;
            replacement.Children.Add(existingSymbol);
        }

        private static bool IsOperator(Token token)
        {
            return !(token.Type == TokenType.SymLit || token.Type == TokenType.Variable);
        }

        private static ExpressionNode CreateNode(Token token, Node parent)
        {
            switch (token.Type)
            {
                case TokenType.SymLit:
                    return new LiteralNode(parent, token.Value);

                case TokenType.Variable:
                    return new VariableNode(parent, token.Value);

                case TokenType.Plus:
                    return new AdditionNode(parent);

                default:
                    throw new Exception(); // TODO: what?
            }
        }
    }
}

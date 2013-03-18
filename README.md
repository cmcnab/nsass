## NSass ##

This project is a port of Sass to .NET.   The intent is to be able to use Sass in a fully .NET environment, like in the ASP.NET Bundle pipeline for example, without having to fire up IronRuby or an external Ruby interpreter.

For the first iteration the focus is only on the SCSS syntax and right now it only supports the very basic features of nesting rules and properties and simple variable replacement.  More to come soon.

### TODO ###


- Expressions
  - Data types
  - Conversions
- Built-in functions
- Interpolation
- Variable defaults
- Parent selectors
- Comments
- @import
- @extend
  - Placeholder selectors
- @mixin/@include
- Old syntax
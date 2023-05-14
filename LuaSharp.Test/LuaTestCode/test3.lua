-- Lua Test Code
print("Hello, World!")

-- Test string escape function
local str = "This is a \"test\" string with \\ some \n escaped \t characters."
print("Original string: " .. str)
str = escape(str)
print("Escaped string: " .. str)
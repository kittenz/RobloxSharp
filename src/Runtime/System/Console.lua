--!strict

local Console = {}
Console.__index = Console

function Console.WriteLine(value: any)
	print(tostring(value))
end

return Console

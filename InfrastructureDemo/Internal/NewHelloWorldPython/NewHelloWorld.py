import sys

name=sys.argv[1]
repeat_count=int(sys.argv[2])
for i in range(repeat_count):
	print(str(i) + " Hello world " + name + "!")
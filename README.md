# normalize
Command line tool to easily normalize line ending in files and folders

------------


### Command line arguments

`-p` or `--path` - File or directory path to normalize

`-t` or `--type` - Line ending type. Possible values are CR, LF or CRLF 

`-s` or `--search` - Search pattern to normalize only specific files in the directory. 


### Usage examples

You can normalize specific file:
```
dotnet normalize.dll -p "c:\project\src\file.txt" -t CRLF
```

Or normalize all files in the directory:
```
dotnet normalize.dll -p "c:\project\src" -t CRLF
```

Or use search pattern to normalize only specific files: 
```
dotnet normalize.dll -p "c:\project\src" -t CRLF -s ".cs,.ts,.txt"
```


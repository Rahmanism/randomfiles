# RandomFiles

The goal is to copy random files from a given path till it reaches the specified size to a destination.  
Another function is to delete random files in the given path till it reaches the specified size.  


---  


### Syntax:

To copy:
```
    randomfiles <source_folder> <destination> [--size nn]
```

To delete:
```
    randomfiles <source_folder> [--size nn] --delete
```

* The default size is 1024.
* The size is in MB.

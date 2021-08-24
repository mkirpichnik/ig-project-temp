
Scripts
========================

Config
-------------------------
Project folder contains ***Chrome-bin.rar***. It's local version of chrome browser which is ***works approporiate on the remote web-server*** with those scripts.

Extract archive on the machine to ***C:\\inetpub\\wwwroot\\Scripts\\Chrome-bin*** or some other directory, but be aware that *binary_location* in each script specified to approporiate folder. 

```python
chrome_options.binary_location = "C:\\inetpub\\wwwroot\\Scripts\\Chrome-bin\\chrome.exe"
```

All the information about user-session contains in folder:
```python
chrome_options.add_argument("--user-data-dir=C:/User_Data/" + username) 
```

Build
-------------------------
To generate .exe file from .py use the following command:

```code
pyinstaller --onefile --script-name.py
```

Response
-------------------------


Scripts have the same structure of result model:
```json
{
    "result": "unique type of response for each script's job",
    "hasError": "true/false",
    "error": "full information about error"
}
```

Supported scripts
-------------------------







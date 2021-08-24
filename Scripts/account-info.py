# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import json
import pickle

from selenium import webdriver
import sys
import re

result = {
    'result': '',
    'hasError': '',
    'error': ''
}

jsonPageSuffix = "/?__a=1"

try:
    userData = sys.argv[1]

    chrome_options = webdriver.ChromeOptions()
    chrome_options.add_argument("--user-data-dir=C:/User_Data/" + userData)
    chrome_options.add_argument("--profile-directory=Default")
    chrome_options.binary_location = "C:\\inetpub\\wwwroot\\Scripts\\Chrome-bin\\chrome.exe"

    driver = webdriver.Chrome(executable_path="chrome_driver\\chromedriver.exe", options=chrome_options)

    userName = sys.argv[2]

    driver.get("http://www.instagram.com/" + userName + jsonPageSuffix)

    page = driver.page_source
    sharedData = re.search("<pre.*>(.*|\n)<\/pre>", page)
    sharedData = sharedData.group(1)
    sharedDataJson = json.loads(sharedData)

    userInfo = sharedDataJson['graphql']
    userInfo = userInfo['user']

    data = {
        'username': userInfo['username'],
        'fullName': userInfo['full_name'],
        'follow': userInfo['edge_follow']['count'],
        'following': userInfo['edge_followed_by']['count'],
        'postsCount': userInfo['edge_owner_to_timeline_media']['count'],
        'isPrivate': userInfo['is_private'],
        'profilePicUrl': userInfo['profile_pic_url'],
        'profilePicUrlHD': userInfo['profile_pic_url_hd']
    }

    result = {
        'result': data,
        'hasError': 'false'
    }

    driver.quit()
except Exception as e:
    result = {
        'hasError': 'true',
        'error': str(e)
    }

resultJson = json.dumps(result)
print(resultJson)
sys.stdout.flush()
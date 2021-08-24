# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import json
import pickle

from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
import time
import sys
import re

# def scroll_page(scrollTimes):
#     for j in range(0, scrollTimes):
#         driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
#         time.sleep(5)
#         pass

# def get_posts_on_page(postsCount):
#     def get_posts_on_page_internal():
#         anchors = driver.find_elements_by_tag_name('a')
#         anchors = [a.get_attribute('href') for a in anchors]
#         anchors = [a for a in anchors if str(a).startswith("https://www.instagram.com/p/")]
#         return anchors
#
#     posts = get_posts_on_page_internal()
#
#     while len(posts) < postsCount:
#         scroll_page(1)
#         new_posts = get_posts_on_page_internal()
#
#         if len(new_posts) == len(posts):
#             break
#
#         posts.extend(new_posts)
#         posts = list(set(posts))
#
#     posts = posts[0:postsCount]
#     return posts

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

    username = sys.argv[2]

    postsCount = int(sys.argv[3])

    driver.get("http://www.instagram.com/" + username + jsonPageSuffix)
    page = driver.page_source

    sharedData = re.search("<pre.*>(.*|\n)<\/pre>", page)
    sharedData = sharedData.group(1)
    sharedDataJson = json.loads(sharedData)

    postsInfo = sharedDataJson['graphql']
    postsInfo = postsInfo['user']
    postsInfo = postsInfo['edge_owner_to_timeline_media']
    postsInfo = postsInfo['edges']

    results = [{
        'ownerUsername': username,
        'postLink': 'https://www.instagram.com/p/' + r['node']['shortcode'],
        'postId': r['node']['shortcode'],
        'orderNumber': idx
    } for idx, r in enumerate(postsInfo)]

    result = {
        'result': results,
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
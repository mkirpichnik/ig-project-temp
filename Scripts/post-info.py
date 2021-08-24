# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import json
import pickle

from selenium import webdriver
import sys
import re

def get_attribute(data, attributeName):
    try:
        return data[attributeName]
    except:
        return ''

def fill_post_content_info(content, data):
    content['id'] = data['id']
    content['type'] = data['__typename']
    content['displayUrl'] = data['display_url']
    content['isVideo'] = data['is_video']

    taggedUsers = data['edge_media_to_tagged_user']['edges'];
    if len(taggedUsers) > 0:
        content['tagged'] = [node['node']['user']['username'] for node in taggedUsers]

    propertyData = get_attribute(get_attribute(data, 'owner'), 'username')
    if propertyData:
        content['ownerUsername'] = propertyData

    propertyData = get_attribute(data, 'taken_at_timestamp')
    if propertyData:
        content['createdTimeStamp'] = propertyData

    propertyData = get_attribute(get_attribute(data, 'edge_media_to_parent_comment'), 'count')
    if propertyData:
        content['commentsCount'] = propertyData

    propertyData = get_attribute(get_attribute(data, 'edge_media_preview_like'), 'count')
    if propertyData:
        content['likesCount'] = propertyData

    if content['isVideo']:
        content['viewsCount'] = data['video_view_count']
        content['videoUrl'] = data['video_url']

    return content

result = {
    'result': '',
    'hasError': '',
    'error': ''
}
driver = "NULL"
jsonPageSuffix = "/?__a=1"

try:
    userData = sys.argv[1]

    chrome_options = webdriver.ChromeOptions()
    chrome_options.add_argument("--user-data-dir=C:/User_Data/" + userData)
    chrome_options.add_argument("--profile-directory=Default")
    chrome_options.binary_location = "C:\\inetpub\\wwwroot\\Scripts\\Chrome-bin\\chrome.exe"

    driver = webdriver.Chrome(executable_path="chrome_driver\\chromedriver.exe", options=chrome_options)

    postId = sys.argv[2]

    driver.get("http://www.instagram.com/p/" + postId + jsonPageSuffix)
    page = driver.page_source

    sharedData = re.search("<pre.*>(.*|\n)<\/pre>", page)
    sharedData = sharedData.group(1)
    sharedDataJson = json.loads(sharedData)

    postInfo = sharedDataJson['graphql']
    postInfo = postInfo['shortcode_media']

    postData = fill_post_content_info({}, postInfo)

    if postData['type'] == 'GraphSidecar':
        childrenData = postInfo['edge_sidecar_to_children']['edges']
        children = [fill_post_content_info({}, item['node']) for item in childrenData]
        postData['children'] = children

    result = {
        'result': postData,
        'hasError': 'false'
    }
except Exception as e:
    result = {
        'hasError': 'true',
        'error': str(e)
    }

driver.quit()
resultJson = json.dumps(result)
print(resultJson)
sys.stdout.flush()
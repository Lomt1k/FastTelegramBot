# FastTelegramBot

## How this project came about:

Initially, I started using Telegram.Bot API for my personal project. I soon noticed that JSON serialization and deserialization could be greatly optimized.

What the normal work of the bot looks like:
1. Your bot receives updates from telegram servers in JSON format.
2. You need to deserialize this JSON into an update object.
3. The internal logic of your bot works, as a result of which, in most cases, you will need to send a response to the incoming update. Usually a message.
4. You serialize your response into a JSON and send it to telegram servers.
5. Telegram sends another JSON to your request, which contains all the information about the message you sent.
6. You again deserialize the incoming JSON into a message object (although in most cases you would only need to get the id of the sent message).

Each JSON serialization and deserialization occurs with the help of Telegram.Bot using reflection. At the same time, all fields from the incoming JSON are completely deserialized.

This project aims to significantly optimize the serialization and deserialization of objects, which can significantly reduce the load on RAM and CPU.

## Due to what:
1. Manual JSON serialization without using reflection, filling only the really necessary fields.
2. Manual JSON deserialization without using reflection, with reading only really necessary fields.

## Important:
Implemented only those methods and functions that I needed to work on a real project. If you found this repository and are interested in it, then you can always expand this functionality if you wish.
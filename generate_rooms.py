import numpy as np
import pandas as pd
import random

df_hotels = pd.read_csv("hotels_v2.csv")
stars = df_hotels["stars"]

avg_daily_price = 103.25  # u dolarima - podatak iz 2020.godine

if __name__ == "__main__":
    df_rooms = pd.DataFrame()

    # u dolarima, +10 za half, +30 za full, +50 za all_inclusive --- ovisi i o broju zvjezdica hotela
    if "half_board" not in df_hotels:
        df_hotels["half_board"] = np.round((stars-2)*10 + np.random.normal(0, 1),1)
    if "full_board" not in df_hotels:
        df_hotels["full_board"] = np.round((stars-2)*20 + np.random.normal(0, 2),1)
    if "all_inclusive" not in df_hotels:
        df_hotels["all_inclusive"] = np.round((stars-2)*30 + np.random.normal(0, 3),1)

    df_hotels.to_csv("hotels_v3.csv", index=False)

    for row in df_hotels.itertuples():  # prodi po retcima=hotelima
        city = row[1]
        hotel_name = row[2]
        star = row[3]
        plus_minus = np.random.normal(0, 10)  # centar je 0, stand. dev = 10
        for i in range(10):  # 10 soba za svaki hotel
            beds = np.random.normal(2, 0.5)  # cca 1-4 kreveta, najcesce 2 i 3
            beds = int(np.ceil(beds))

            # avg_daily_price je pocetna cijena za kategoriju 3 zvijezdice i +200 dolara za kategoriju vise
            # 1 krevet nema dodatne naknade, 2 kreveta=+30$, 3 kreveta=+60$ itd.
            # plus 30 dolara za dodatni krevet
            daily_price = avg_daily_price + (star - 3) * 200 + (beds - 1) * 30 + plus_minus

            room = {"city":city, "hotel_name": hotel_name,"beds": beds, "daily_price": np.round(daily_price,1), "room_number": i+1}
            df_rooms = df_rooms.append(room, ignore_index=True)

    # print(df_rooms.head(20), plus_minus)
    df_rooms.to_csv("rooms.csv", index=False)

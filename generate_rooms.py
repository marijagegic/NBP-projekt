import numpy as np
import pandas as pd

""" ovo odkomentirat kad se kreiraju hoteli"""
#df_hotels = pd.read_csv("hoteli.csv")
#stars = df_hotels["stars"]

stars = [np.random.randint(3,6) for _ in range(6*10)]  # dummy podaci, zvjezdice hotela

avg_daily_price = 103.25 # u dolarima - podatak iz 2020.godine

if __name__ == "__main__":
    df = pd.DataFrame()
    for star in stars:  # za svaki hotel s tom zvjezdicom -> i treba još puta broj soba(možda 3?)
        beds = np.random.normal(2, 0.5)  # cca 1-4 kreveta, najcesce 2 i 3
        beds = int(np.ceil(beds))

        plus_minus = np.random.normal(0, 10)  # centar je 0, stand. dev = 10
        # avg_daily_price je pocetna cijena za kategoriju 3 zvijezdice i +200 dolara za kategoriju vise
        # plus 30 dolara za dodatni krevet
        daily_price = avg_daily_price + (star-3)*200 + (beds-1)*30 + plus_minus

        # TODO u hoteli.csv dodati cjenik: cca 0 za polupansion, 50 za puni, 100 za all_inclusive (ali ovisi i o broju zvjezdica)

        df2 = {"stars":star, "beds": beds, "daily_price": int(daily_price), "occupied": 0}
        df = df.append(df2, ignore_index=True)

    print(df)
    #df.to_csv("rooms.csv", index=False)
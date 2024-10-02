import React, { useEffect, useState } from 'react';
import './style/DateDisplay.css';
import dayjs from './dayjs';
var sprintf = require('sprintf-js').sprintf;



const InteractiveForm = ({user, initialUserTimes, initialOtherTimes, requestToken, postDateApi}) => {

    type CellCallback = (date: dayjs.Dayjs, e: React.MouseEvent<HTMLElement>) => void;

    console.log(Array.from(initialUserTimes, (k: number) => dayjs(k).toString()));

    const [baseDate, setBaseDate] = useState<dayjs.Dayjs>(dayjs().startOf('day'));
    const [userTimeSlots, setUserTimeSlots] = useState<Map<number, Set<string>>>(ingestUserTimes(initialUserTimes));
    const [otherTimeSlots, setOtherTimeSlots] = useState<Map<number, Set<string>>>(ingestOtherTimes(initialOtherTimes));

    function ingestUserTimes(initialUserTimes): Map<number, Set<string>> {
        if (!Array.isArray(initialUserTimes)) throw Error("initialUserTimes is not array");
        let injestedTimes = new Map<number, Set<string>>();
        initialUserTimes.forEach((d) => {
            if (!dayjs(d).isValid()) throw Error('date in initialUserTimes invalid: ' + String(d));
            injestedTimes.set(dayjs(d).valueOf(), new Set<string>(user));
        });
        console.log("initial user timeslots: " + Array.from(injestedTimes.keys(), (k: number) => dayjs(k).toString()));
        return injestedTimes;
    }

    function ingestOtherTimes(initialOtherTimes): Map<number, Set<string>> {
        return new Map<number, Set<string>>();
    }

    const PostDatesCallback = async (date: dayjs.Dayjs, e: React.MouseEvent<HTMLElement>) => {


        if (userTimeSlots.has(date.valueOf())) {
            userTimeSlots.delete(date.valueOf());
            setUserTimeSlots(new Map(userTimeSlots));
        }
        else setUserTimeSlots(new Map(userTimeSlots.set(date.valueOf(), new Set<string>(user))));

        console.log("callback for date: " + date.toString());

        console.log("selected " + JSON.stringify(Array.from(userTimeSlots.keys(), (k) => dayjs(k))))

        const response = await fetch(postDateApi, {
            method: "POST",
            headers: {
                RequestVerificationToken: requestToken,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(Array.from(userTimeSlots.keys()))
        });

        if (!response.ok) {
            console.log(`Request Failed: ${response.status}`);
        }
    }


    //function genDateCells(hour: number): Array<JSX.Element> {
    //    return Array.from(Array(7).keys(), day => {
    //        let refDate = baseDate.add(day, 'day').hour(hour);
    //        return <td
    //            className={'timeSlot ' + (userTimeSlots.has(refDate.valueOf()) ? 'selectedTime' : 'deselectedTime') }
    //            key={refDate.valueOf()}
    //        ></td>
    //    });
    //}

    function genTimeSlotGrid(refSlots: Map<number, Set<string>>, callback: CellCallback) {
        return Array.from(Array(24).keys(), h => {
            let hour = h + 1;
            return <tr className='timeRow' key={hour}>
                <td className="timeSlot">
                    <p className="timeRowHeader">
                        {hour > 12 ? sprintf('%d pm', hour - 12) : sprintf('%d am', hour)}
                    </p>
                </td>
                {   Array.from(Array(7).keys(), day => {
                        let cellDate = baseDate.add(day, 'day').hour(hour);
                        return <td
                            className={'timeSlot ' + (refSlots.has(cellDate.valueOf()) ? 'selectedTime' : 'deselectedTime')}
                            key={cellDate.valueOf()}
                            data-date={cellDate.toString()}
                            {... (callback != null ? { onClick : callback.bind(null, cellDate) } : {})}
                        ></td>
                    })
                }
            </tr>
        });
    }

    const dateHeaderCells: Array<JSX.Element> = Array.from(Array(7).keys(), n =>
        <td className='timeSlot' key={n}>
            <p className='timeSlotHeader'> {baseDate.add(n, 'day').format('dddd, MMM D')}</p>
        </td>
    ); 

    return (
        <div className="dateDisplayContainer"> 
            <div className="dateTableWrapper">
                <h3>Your Times</h3>
                <table className="dateTimeTable">
                    <thead>
                        <tr>
                            <td />
                            {dateHeaderCells}
                        </tr>
                    </thead>
                    <tbody>
                        {genTimeSlotGrid(userTimeSlots, PostDatesCallback)}
                    </tbody>
                </table>
            </div >
            <div className="dateTableWrapper">
                {/*<h3>Other Users: @otherDateTimes.Keys.Aggregate("", (s, next) => $"{s} {next}")</h3>*/}
                <table className="dateTimeTable">
                    <thead>
                        <tr>
                            <td />
                            {dateHeaderCells}
                        </tr>
                    </thead>
                    <tbody>
                        {genTimeSlotGrid(otherTimeSlots, null)}
                    </tbody>
                </table>
            </div>
        </div >
    );
};

export default InteractiveForm;
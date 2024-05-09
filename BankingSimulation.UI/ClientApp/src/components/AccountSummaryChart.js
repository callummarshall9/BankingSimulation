import React, { Component } from 'react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

export default class AccountSummaryChart extends Component {
    static getColour(entry) {
        const colours = ["red", "green", "blue", "purple", "pink", "black", "orange"];
        let charSum = entry.accountId.split('')
            .map(char => char.charCodeAt(0))
            .reduce((current, previous) => previous + current);
        
        return colours[charSum % (colours.length + 1)];
    }

    getLegend() {
        let distinctAccountIds = [...new Set(this.props.chartData.map(cd => cd.accountId))];

        return distinctAccountIds.map(dai => {
            let item = this.props.chartData.filter(cd => cd.accountId === dai)[0];
    
            return {
                id: item.computedAccountInfo,
                type: "square",
                color: AccountSummaryChart.getColour(item),
                value: item.computedAccountInfo
            }
        });
    }

    render() {
        let copy = JSON.parse(JSON.stringify(this.props.chartData));


        copy.map(cd => cd.fill = AccountSummaryChart.getColour(cd));
        console.log(copy);

          return (
            <div style={{ minWidth: "800px", minHeight: "600px", width: "100%", height: "600px" }}>
                <ResponsiveContainer width="100%" height="100%">
                <BarChart
                    width={500}
                    height={300}
                    data={copy}
                    margin={{
                    top: 5,
                    right: 30,
                    left: 20,
                    bottom: 5,
                    }}
                >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="calendarEventName" />
                    <YAxis />
                    <Tooltip formatter={(_1, _2, props) => "+" + props.payload.incomings + " " + props.payload.outgoings + " = " + props.payload.net + " (" + props.payload.computedAccountInfo + ")"}/>
                    <Legend payload={this.getLegend()} />
                    <Bar dataKey="net" />
                   
                </BarChart>
                </ResponsiveContainer>
            </div>
          );
    }
  }
  